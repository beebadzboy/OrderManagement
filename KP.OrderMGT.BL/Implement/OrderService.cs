using KP.OrderMGT.BL.DBModel;
using KP.OrderMGT.BL.ServiceModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Transactions;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace KP.OrderMGT.Service
{
    public class OrderService : IOrderService
    {
        private OrderDataClassesDataContext _omDB;
        private POSAirPortClassesDataContext _posDB;
        public OrderService() { }
        public OrderService(OrderDataClassesDataContext omDB)
        { _omDB = omDB; }
        public OrderService(POSAirPortClassesDataContext posDB)
        { _posDB = posDB; }
        public OrderService(OrderDataClassesDataContext omDB, POSAirPortClassesDataContext posDB)
        { _omDB = omDB; _posDB = posDB; }

        public string GetConnectionPOSAirport(string airport_code)
        {
            var connObj = _omDB.config_connections.FirstOrDefault(x => x.cn_code == airport_code);
            if (connObj == null)
            {
                throw new System.ArgumentException("message", nameof(connObj));
            }

            var connectionString = "Data Source=" + connObj.cn_server.Trim() + ";Initial Catalog=" + connObj.cn_database.Trim() + ";Persist Security Info=True;User ID=" + connObj.cn_uid.Trim() + ";Password=" + connObj.cn_pwd.Trim() + ";";
            return connectionString;
        }

        public string GetConnectionPOSOrder(string order_no)
        {
            var sessionObj = _omDB.order_sessions.FirstOrDefault(x => x.sale_order_no == order_no);
            if (sessionObj == null)
            {
                throw new System.ArgumentException(order_no + ": data not found.", nameof(sessionObj));
            }

            var airportCoode = sessionObj.order_headers.Select(x => x.airport_code).FirstOrDefault();
            if (airportCoode == null)
            {
                throw new System.ArgumentException(order_no + ": data not found.", nameof(sessionObj));
            }

            return GetConnectionPOSAirport(airportCoode);
        }

        public OrderSession GetOrderOnline(string order_no)
        {
            var saleList = new List<SaleQueue>();
            var saleObj = _omDB.order_sessions.FirstOrDefault(x => x.sale_order_no == order_no);
            if (saleObj == null)
            {
                throw new System.ArgumentException("data not found.", nameof(saleObj));
            }

            return new OrderSession(saleObj);
        }

        public List<OrderSession> GetOrderOnlineList(string airport_code, int? skip, int? take)
        {
            var saleList = new List<OrderSession>();
            if (skip.HasValue && take.HasValue)
            {
                var saleObj = _omDB.order_sessions.Where(x => x.order_headers.FirstOrDefault(y => y.airport_code == airport_code) != null).Skip(skip.Value).Take(take.Value).ToList();
                foreach (var item in saleObj)
                {
                    var obj = new OrderSession(item);
                    saleList.Add(obj);
                }
            }
            else
            {
                var saleObj = _omDB.order_sessions.Where(x => x.order_headers.FirstOrDefault(y => y.airport_code == airport_code) != null).ToList();
                foreach (var item in saleObj)
                {
                    var obj = new OrderSession(item);
                    saleList.Add(obj);
                }
            }

            return saleList;
        }

        private double RunningNumber(POSAirPortClassesDataContext _posDB, runno_machine last_number, string type)
        {
            double number = 1;
            if (last_number == null)
            {
                throw new ObjectNotFoundException("error machine no. for running document number missing in POS Database");
            }
            else
            {
                if (type == "taxabb")
                {
                    number = last_number.taxabb + 1;
                    if (number > 99999)
                    {
                        number = 1;
                    }
                }
                else
                {
                    number = last_number.reciept + 1;
                    if (number > 99999)
                    {
                        number = 1;
                    }
                }
            }

            return number;
        }

        public OrderSession SaveOrderOnline(POSAirPortClassesDataContext _posDB, OrderHeader order)
        {
            if (string.IsNullOrWhiteSpace(order.Billing.PassportNo))
            {
                throw new System.ArgumentException("message", nameof(order.Billing.PassportNo));
            }

            var connObj = _omDB.config_connections.FirstOrDefault(x => x.cn_code == order.Flight.AirportCode);
            if (connObj == null)
            {
                throw new System.ArgumentException("message", nameof(order.Flight.AirportCode));
            }

            var last_number = _posDB.runno_machines.FirstOrDefault(x => x.machine_no.Trim() == connObj.ref_machine_no.Trim());
            if (last_number == null)
            {
                throw new System.ArgumentException("message", nameof(last_number));
            }

            var order_header = _posDB.df_header_onls.FirstOrDefault(x => x.OnlineNo == order.NewOrder.OrderNo);
            if (order_header != null)
            {
                throw new System.ArgumentException("order is duplicate.", nameof(order_header.OnlineNo));
            }

            var order_result = new OrderSession();

            //var option = new TransactionOptions();
            //option.IsolationLevel = IsolationLevel.ReadCommitted;
            //option.Timeout = TimeSpan.FromMinutes(2);
            //using (TransactionScope tran = new TransactionScope(TransactionScopeOption.Required, option))
            //{
            //    try
            //    {
            // pos airport save
            char sale_status = Char.Parse("R");
            if (order.Flight.Terminal == "D")
            {
                sale_status = Char.Parse("D");
            }

            var docno = RunningNumber(_posDB, last_number, "taxabb");
            var runno = RunningNumber(_posDB, last_number, "revdoc");

            last_number.taxabb = docno;
            last_number.reciept = runno;
            _posDB.SubmitChanges();

            CultureInfo ci = CultureInfo.InvariantCulture;
            // new df_header_onl
            df_header_onl new_order = new df_header_onl
            {
                branch_no = connObj.cn_branch_no,
                data_date = DateTime.Now,
                area_code = connObj.ref_area_no,
                loc_code = connObj.ref_loc_no,
                machine_no = connObj.ref_machine_no,
                doc_no = docno.ToString("00000"),
                shift_no = 1,
                cust_type = "",
                flight_code = order.Flight.FlightCode,
                flight_date = order.Flight.Date,
                flight_time = order.Flight.Time.Time24,
                country_code = order.Billing.CountryCode,
                cust_name = order.Billing.FirstName + " " + order.Billing.LastName,
                passport_no = order.Billing.PassportNo,
                member_id = order.NewOrder.MemberID,
                shopping_card = "",
                cashier_code = "online",
                sale_code = "online",
                update_date_sale = null,
                add_datetime = DateTime.Now,
                update_datetime = DateTime.Now,
                user_add = "online",
                user_update = "online",
                //time_stamp = "",
                void_status = false,
                cancel_to_doc = "",
                cancel_to_date = null,
                cancel_to_mac = "",
                runno = runno.ToString("00000"),
                data_time = DateTime.Now.ToString("HH:mm", ci),
                trans_date = DateTime.Now.Date,
                //rec_no = 0,
                tour_barcode = "",
                tour_code = "",
                machine_tax = connObj.ref_machine_tax,
                rcv_data_date = null,
                rcv_loc_code = "",
                rcv_machine_no = "",
                rcv_doc_no = "",
                rcv_datetime = null,
                sale_status = sale_status,
                //DiscAuthUser = "",
                //CardTypeCode = "",
                //EmBossID = "",
                //CardTypeID = "",
                //PersonalID = "",
                //Gender = "",
                //BirthDate = "",
                //PassportExpire = "",
                //rcv_status = "",
                //LVHeaderKey = "",
                //DeliveryType = "",
                //DeliveryAuth = "",
                //ReferDate = DateTime.Now.Date,
                //ReferMac = "online",
                //ReferDoc = order.NewOrder.OrderNo,
                //RCCode = "",
                //AlipaySession = "",
                //WeChatSession = "",
                //Promoter = "",
                //DFA = "",
                OnlineNo = order.NewOrder.OrderNo,
                TerminalCode = order.Flight.Terminal,
                LastStatus = "003".ToString()
            };
            _posDB.df_header_onls.InsertOnSubmit(new_order);
            _posDB.SubmitChanges();

            // new df_tran_onl and new pdiscount_onl
            var item_code_list = order.Items.Select(x => x.MaterialCode).ToList();
            var master_article = _posDB.vArticleMCs.Where(x => item_code_list.Contains(x.ArticleCode)).ToList();
            var line_disc_no = 0;
            foreach (var item in order.Items.Select((value, index) => new { Value = value, Index = index }))
            {
                var item_code = master_article.FirstOrDefault(x => x.ArticleCode == item.Value.MaterialCode);

                df_trans_onl new_item = new df_trans_onl() {
                    branch_no = new_order.branch_no.Trim(),
                    data_date = DateTime.Now,
                    area_code = new_order.area_code.Trim(),
                    loc_code = new_order.loc_code,
                    machine_no = new_order.machine_no.Trim(),
                    doc_no = docno.ToString("00000"),
                    line_no = item.Index + 1,
                    item_code = item_code.GTIN,
                    bar_code = "",
                    mat_code = item.Value.MaterialCode,
                    quantity = item.Value.Quantity,
                    selling_price = item.Value.SellingPrice,
                    curr_code = "",
                    amount = item.Value.Amount,
                    discount = item.Value.Discount,
                    net = item.Value.Net,
                    vat = 0,
                    vat_code = "2",
                    vat_rate = 0,
                    disc_code = "",
                    disc_rate = item.Value.DiscountRate,
                    promo_code = item.Value.PromoCode,
                    staff_code = new_order.sale_code,
                    staff_comm_rate = 0,
                    cancel_status = false,
                    add_datetime = DateTime.Now,
                    update_datetime = DateTime.Now,
                    user_add = new_order.sale_code,
                    user_update = new_order.sale_code,
                    //time_stamp = "",
                    line_cancel = false,
                    discount2 = item.Value.SPDiscount,
                    disc_rate2 = item.Value.SPDiscountRate,
                    net2 = item.Value.TotalNet,
                    vat2 = 0
                };

                _posDB.df_trans_onls.InsertOnSubmit(new_item);

                if (item.Value.Discount > 0)
                {
                    line_disc_no = line_disc_no + 1;
                    df_pdiscount_onl new_pdiscount = new df_pdiscount_onl()
                    {
                        branch_no = new_order.branch_no.Trim(),
                        data_date = DateTime.Now,
                        machine_no = new_order.machine_no.Trim(),
                        doc_no = docno.ToString("00000"),
                        plu_line_no = new_item.line_no,
                        disc_line_no = line_disc_no,
                        disc_rate = (decimal)new_item.disc_rate,
                        disc_per = (decimal)new_item.disc_rate > 0 ? (decimal)new_item.discount : 0,
                        disc_amount = (decimal)new_item.disc_rate <= 0 ? (decimal)new_item.discount : 0,
                        promo_code = item.Value.PromoCode,
                        bybill_flag = false,
                        bybill_runno = 1,
                        disc_type = 1,
                        del_flag = null,
                        cancel_status = false,
                        add_datetime = DateTime.Now,
                        update_datetime = DateTime.Now,
                        user_add = new_item.user_add,
                        user_update = new_item.user_update,
                        //time_stamp = "",
                        QRCode = new Guid("00000000-0000-0000-0000-000000000000"),
                        method_code = "",
                        subsidize = 0
                    };

                    _posDB.df_pdiscount_onls.InsertOnSubmit(new_pdiscount);
                    _posDB.SubmitChanges();
                }

                if (item.Value.SPDiscount > 0)
                {
                    line_disc_no = line_disc_no + 1;
                    df_pdiscount_onl new_pdiscount = new df_pdiscount_onl()
                    {
                        branch_no = new_order.branch_no,
                        data_date = DateTime.Now,
                        machine_no = new_order.machine_no,
                        doc_no = docno.ToString("00000"),
                        plu_line_no = new_item.line_no,
                        disc_line_no = line_disc_no,
                        disc_rate = (decimal)item.Value.SPDiscountRate,
                        disc_per = (decimal)item.Value.SPDiscountRate > 0 ? (decimal)item.Value.SPDiscount : 0,
                        disc_amount = (decimal)item.Value.SPDiscount <= 0 ? (decimal)item.Value.SPDiscount : 0,
                        promo_code = item.Value.SPPromoCode,
                        bybill_flag = true,
                        bybill_runno = 2,
                        disc_type = 2,
                        del_flag = null,
                        add_datetime = DateTime.Now,
                        update_datetime = DateTime.Now,
                        user_add = new_item.user_add,
                        user_update = new_item.user_update,
                        //time_stamp = "",
                        //QRCode = "",
                        //method_code = "",
                        //subsidize = ""
                    };

                    _posDB.df_pdiscount_onls.InsertOnSubmit(new_pdiscount);
                    _posDB.SubmitChanges();
                }
            }

            // new df_payment_onl
            var master_paymath = _posDB.df_paymeths.ToList();
            foreach (var payment in order.Payments.Select((value, index) => new { Value = value, Index = index }))
            {
                decimal amt_curr = 0;
                var paymath = master_paymath.FirstOrDefault(x => x.method_code.Trim() == payment.Value.Code.Trim());
                if (paymath != null)
                {
                    if (paymath.check_voucher || paymath.is_cashcard)
                    {
                        amt_curr = 0;
                    }
                    else
                    {
                        amt_curr = payment.Value.Amount * 1;
                    }
                }

                df_payment_onl new_payment = new df_payment_onl()
                {
                    branch_no = new_order.branch_no.Trim(),
                    data_date = DateTime.Now,
                    machine_no = new_order.machine_no.Trim(),
                    doc_no = docno.ToString("00000"),
                    line_no = payment.Index + 1,
                    method_code = payment.Value.Code.Trim(),
                    payment_date = DateTime.Now,
                    amount = payment.Value.Amount,
                    amount_round = 0,
                    amount_curr = amt_curr,
                    curr_code = "THB",
                    curr_rate = 1,
                    cashier_code = new_order.cashier_code.Trim(),
                    posid = new_order.machine_no.Trim(),
                    cred_card_no = "",
                    cred_card_name = "",
                    expiry_date = null,
                    approve_code = "",
                    del_flag = null,
                    add_datetime = DateTime.Now,
                    update_datetime = DateTime.Now,
                    user_add = new_order.user_add,
                    user_update = new_order.user_update,
                    //time_stamp = "",
                    amount2 = payment.Value.Amount,
                    amount_curr2 = amt_curr,
                    BankOfEDC = "",
                    AliBarcode = "",
                    AliMerchantID = "",
                    AliTransID = "",
                    AlipayCancel = false
                };


                _posDB.df_payment_onls.InsertOnSubmit(new_payment);
                _posDB.SubmitChanges();
            }

            // order_session db save
            order_result = SaveSessionOrder(order, new_order);


            //    tran.Complete();
            //}
            //catch (Exception ex)
            //{
            //    tran.Dispose();
            //    throw ex;
            //}
            //}


            return order_result;
        }

        private OrderSession SaveSessionOrder(OrderHeader order, df_header_onl new_order)
        {
            // order_session db save
            var new_session_order = new order_session();
            new_session_order.session_guid = Guid.NewGuid();
            new_session_order.sale_agent_code = order.NewOrder.AgentCode;
            new_session_order.sale_order_no = order.NewOrder.OrderNo;
            new_session_order.sale_order_status = "Create";
            new_session_order.sale_invoice_no = order.NewOrder.InvoiceNo;
            new_session_order.sale_platform = "online";
            var obj_key = new OrderKey();
            obj_key.Dete = new_order.data_date.ToString("yyyy-MM-dd HH:mm");
            obj_key.DocNO = new_order.doc_no.ToString();
            obj_key.MacNo = new_order.machine_no.Trim();
            new_session_order.pos_order_key = JsonConvert.SerializeObject(obj_key);
            new_session_order.pos_order_no = new_order.machine_no.Trim() + "-" + new_order.doc_no;
            new_session_order.pos_order_status = new_order.LastStatus;
            new_session_order.pos_invice_no = new_order.machine_no.Trim() + "-" + new_order.runno;
            new_session_order.create_date = DateTime.Now;
            new_session_order.modified_date = DateTime.Now;
            _omDB.order_sessions.InsertOnSubmit(new_session_order);
            _omDB.SubmitChanges();
            // order_tran db save
            var new_tren_order = new order_transaction();
            new_tren_order.create_date = DateTime.Now;
            new_tren_order.session_id = new_session_order.id;
            new_tren_order.datail = "Create Sale Order Online [" + new_session_order.sale_order_no + "] <---> POS Order [" + new_session_order.pos_order_no + "]";
            _omDB.order_transactions.InsertOnSubmit(new_tren_order);
            _omDB.SubmitChanges();

            var order_result = new OrderSession();
            order_result.SessionId = new_session_order.id;
            order_result.SessionGuid = new_session_order.session_guid;
            order_result.SaleOrderNo = new_session_order.sale_order_no;
            order_result.POSOrderNo = new_session_order.pos_order_no;
            order_result.POSInvoiceNo = new_session_order.pos_invice_no;
            order_result.POSStatus = new_session_order.pos_order_status;
            order_result.POSSessionKey = new_session_order.pos_order_key;

            SaveLogInterface(order);

            return order_result;
        }


        private void SaveLogInterface(OrderHeader order)
        {

        }

        public SaleAmountByPassport ValidateAllowSaleOnline(POSAirPortClassesDataContext _posDB, char terminal, string passort, DateTime date, int time)
        {
            var saleObj = _posDB.get_sale_passport_vol2(time, passort, date, terminal).FirstOrDefault();
            if (saleObj == null)
            {
                throw new System.ArgumentException("data not found.", nameof(saleObj));
            }

            return new SaleAmountByPassport(saleObj);
        }

        public List<SaleQueue> SaleQueue(POSAirPortClassesDataContext _posDB, char terminal)
        {
            string[] status = new string[] { "003" };
            var saleList = new List<SaleQueue>();
            var saleObj = _posDB.df_trans_onls.Where(y => y.cancel_status != true && y.df_header_onl.TerminalCode == terminal.ToString() && status.Contains(y.df_header_onl.LastStatus)).GroupBy(p => p.item_code, (key, g) => new { SKU = key, ListData = g.ToList() }).ToList();
            foreach (var sale in saleObj)
            {
                var newSale = new SaleQueue(sale.SKU, sale.ListData);
                saleList.Add(newSale);
            }

            return saleList;
        }

        public OrderSession CancelOrderOnline(POSAirPortClassesDataContext _posDB, string order_no)
        {
            var order = GetOrderOnline(order_no);
            if (order == null)
            {
                throw new ObjectNotFoundException(order_no + " : data not found.");
            }

            var pos_key_order = new OrderKey(order);
            var pos_data = _posDB.df_header_onls.FirstOrDefault(x => x.OnlineNo == order_no);
            if (pos_data == null)
            {
                throw new ObjectNotFoundException(order_no + " : data not found.");
            }

            int status = (int)StatusOrderPOS.CancelCreated;
            pos_data.LastStatus = status.ToString();
            pos_data.update_datetime = DateTime.Now;
            pos_data.user_update = "online";
            _posDB.SubmitChanges();

            return UdpateOrderSession(order_no, StatusOrderPOS.CancelCreated);
        }

        public OrderSession HoleOrderOnline(POSAirPortClassesDataContext _posDB, string order_no)
        {
            var order = GetOrderOnline(order_no);
            if (order == null)
            {
                throw new ObjectNotFoundException(order_no + " : data not found.");
            }

            var pos_key_order = new OrderKey(order);
            var pos_data = _posDB.df_header_onls.FirstOrDefault(x => x.doc_no == pos_key_order.DocNO && x.OnlineNo == order_no);
            if (pos_data == null)
            {
                throw new ObjectNotFoundException(order_no + " : data not found.");
            }

            int status = (int)StatusOrderPOS.HoldOrder;
            pos_data.LastStatus = status.ToString();
            pos_data.update_datetime = DateTime.Now;
            pos_data.user_update = "online";
            _posDB.SubmitChanges();

            return UdpateOrderSession(order_no, StatusOrderPOS.HoldOrder);
        }

        public OrderSession VoidOrderOnline(string order_no)
        {
            return UdpateOrderSession(order_no, StatusOrderPOS.VoidSaved);
        }

        public OrderSession ComplateOrderOnline(string order_no)
        {
            return UdpateOrderSession(order_no, StatusOrderPOS.Complete);
        }

        private OrderSession UdpateOrderSession(string order_no, StatusOrderPOS status)
        {
            var saleList = new List<SaleQueue>();
            var saleObj = _omDB.order_sessions.FirstOrDefault(x => x.sale_order_no == order_no);
            if (saleObj == null)
            {
                throw new System.ArgumentException("data not found.", nameof(saleObj));
            }

            saleObj.pos_order_status = status.ToString();
            saleObj.modified_date = DateTime.Now;
            _omDB.SubmitChanges();

            var tran = new order_transaction();
            tran.create_date = DateTime.Now;
            tran.session_id = saleObj.id;
            tran.datail = "Update Status POS Order ["+ saleObj.pos_order_no + "] : " + status.ToString();
            _omDB.order_transactions.InsertOnSubmit(tran);

            return new OrderSession(saleObj);
        }
    }
}
