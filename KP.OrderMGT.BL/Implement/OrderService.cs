using KP.OrderMGT.BL.DBModel;
using KP.OrderMGT.BL.ServiceModel;
using System;
using System.Data;
using System.Data.Entity.Core;
using System.Globalization;
using System.Linq;
using System.Transactions;

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

        public SaleOnlineByPassport GetOrderOnline(string order_no)
        {
            throw new NotImplementedException();
        }

        public SaleOnlineByPassport GetOrderOnlineList(string airport_code, int? skip, int? take)
        {
            throw new NotImplementedException();
        }

        public SaleOnlineByPassport HoleOrderOnline(OrderHeader order)
        {
            throw new NotImplementedException();
        }

        public SaleOnlineByPassport HoleOrderOnline(string order_no)
        {
            throw new NotImplementedException();
        }


        private string RunningNumber(POSAirPortClassesDataContext _posDB, string machine_no, string type)
        {
            {
                var doc_number = 0.ToString("00000");
                var last_number = _posDB.runno_machines.FirstOrDefault(x => x.machine_no == machine_no);
                if (last_number == null)
                {
                    throw new ObjectNotFoundException("error machine no. for running document number missing in POS Database");
                }
                else
                {
                    double number = 1;
                    if (type == "taxabb")
                    {
                        number = last_number.taxabb + 1;
                        if (number < 99999)
                        {
                            number = 1;
                        }

                        last_number.taxabb = number;
                    }
                    else
                    {
                        number = last_number.reciept + 1;
                        if (number < 99999)
                        {
                            number = 1;
                        }

                        last_number.reciept = number;
                    }


                    _posDB.SubmitChanges();
                    doc_number = number.ToString("00000");
                }

                return doc_number;
            }
        }

        public OrderSession SaveOrderOnline(POSAirPortClassesDataContext _posDB, OrderHeader order)
        {
            var order_session = new OrderSession();
            using (TransactionScope tran = new TransactionScope())
            {
                try
                {
                    // pos airport save

                    char sale_status = Char.Parse("");
                    if (order.Flight.Terminal == "D")
                    {
                        sale_status = Char.Parse("D");
                    }
                    var runno = RunningNumber(_posDB, "online", "taxabb");
                    CultureInfo ci = CultureInfo.InvariantCulture;

                    // new df_header_onl
                    df_header_onl new_order = new df_header_onl
                    {
                        branch_no = "01",
                        data_date = DateTime.Now,
                        area_code = "online",
                        loc_code = "online",
                        machine_no = "OL01",
                        doc_no = runno,
                        shift_no = 1,
                        cust_type = "",
                        flight_code = order.Flight.FlightCode,
                        flight_date = order.Flight.Date,
                        flight_time = order.Flight.Time.Time24,
                        cust_name = order.Billing.FirstName + " " + order.Billing.LastName,
                        passport_no = order.NewOrder.PassportNo,
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
                        //cancel_to_doc = "",
                        //cancel_to_date = null,
                        //cancel_to_mac = "",
                        runno = runno,
                        data_time = DateTime.Now.ToString("HH:mm", ci),
                        trans_date = DateTime.Now.Date,
                        //rec_no = "",
                        tour_barcode = "",
                        tour_code = "",
                        machine_tax = "XXXXX",
                        //rcv_data_date = "",
                        //rcv_loc_code = "",
                        //rcv_machine_no = "",
                        //rcv_doc_no = "",
                        //rcv_datetime = "",
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
                        ReferDate = DateTime.Now.Date,
                        ReferMac = "online",
                        ReferDoc = order.NewOrder.OrderNo,
                        //RCCode = "",
                        //AlipaySession = "",
                        //WeChatSession = "",
                        //Promoter = "",
                        //DFA = "",
                        OnlineNo = order.NewOrder.OrderNo,
                        Status = "E",
                        CurrStatus = "E",
                    };
                    _posDB.df_header_onls.InsertOnSubmit(new_order);

                    // new df_tran_onl and new pdiscount_onl
                    var item_code_list = order.Items.Select(x => x.MaterialCode).ToList();
                    var master_article = _posDB.vArticleMCs.Where(x => item_code_list.Contains(x.ArticleCode)).ToList();
                    var line_disc_no = 0;
                    foreach (var item in order.Items.Select((value, index) => new { Value = value, Index = index }))
                    {
                        var item_code = master_article.FirstOrDefault(x => x.ArticleCode == item.Value.MaterialCode);

                        df_trans_onl new_item = new df_trans_onl() {
                            branch_no = new_order.branch_no,
                            data_date = DateTime.Now,
                            area_code = new_order.area_code,
                            loc_code = new_order.loc_code,
                            machine_no = new_order.machine_no,
                            doc_no = runno,
                            line_no = item.Index + 1,
                            item_code = item_code.GTIN,
                            bar_code = item_code.ArticleCode,
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
                                branch_no = new_order.branch_no,
                                data_date = DateTime.Now,
                                machine_no = new_order.machine_no,
                                doc_no = runno,
                                plu_line_no = new_item.line_no,
                                disc_line_no = line_disc_no,
                                disc_rate = (decimal)new_item.disc_rate,
                                disc_per = (decimal)new_item.disc_rate > 0 ? (decimal)new_item.discount : 0,
                                disc_amount = (decimal)new_item.disc_rate <= 0 ? (decimal)new_item.discount : 0,
                                promo_code = item.Value.PromoCode,
                                bybill_flag = false,
                                bybill_runno = 1,
                                //disc_type = "",
                                del_flag = Char.Parse(""),
                                cancel_status = false,
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
                        }

                        if (item.Value.SPDiscount > 0)
                        {
                            line_disc_no = line_disc_no + 1;
                            df_pdiscount_onl new_pdiscount = new df_pdiscount_onl()
                            {
                                branch_no = new_order.branch_no,
                                data_date = DateTime.Now,
                                machine_no = new_order.machine_no,
                                doc_no = runno,
                                plu_line_no = new_item.line_no,
                                disc_line_no = line_disc_no,
                                disc_rate = (decimal)item.Value.SPDiscountRate,
                                disc_per = (decimal)item.Value.SPDiscountRate > 0 ? (decimal)item.Value.SPDiscount : 0,
                                disc_amount = (decimal)item.Value.SPDiscount <= 0 ? (decimal)item.Value.SPDiscount : 0,
                                promo_code = item.Value.SPPromoCode,
                                bybill_flag = true,
                                bybill_runno = 2,
                                //disc_type = "",
                                del_flag = Char.Parse(""),
                                cancel_status = false,
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
                        }
                    }

                    // new df_payment_onl
                    var master_paymath = _posDB.df_paymeths.ToList();
                    foreach (var payment in order.Payments.Select((value, index) => new { Value = value, Index = index }))
                    {
                        decimal amt_curr = 0;
                        var paymath = master_paymath.FirstOrDefault(x=>x.method_code == payment.Value.Code);
                        if (paymath != null)
                        {
                            if(paymath.check_voucher || paymath.is_cashcard)
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
                            branch_no = new_order.branch_no,
                            data_date = DateTime.Now,
                            machine_no = new_order.machine_no,
                            doc_no = runno,
                            line_no = payment.Index + 1,
                            method_code = payment.Value.Code,
                            payment_date = DateTime.Now,
                            amount = payment.Value.Amount,
                            amount_round = 0,
                            amount_curr = amt_curr,
                            curr_code = "THB",
                            curr_rate = 1,
                            cashier_code = new_order.cashier_code,
                            posid = new_order.machine_no,
                            //cred_card_no = "",
                            //cred_card_name = "",
                            //expiry_date = "",
                            //approve_code = "",
                            //del_flag = "",
                            add_datetime = DateTime.Now,
                            update_datetime = DateTime.Now,
                            user_add = new_order.user_add,
                            user_update = new_order.user_update,
                            //time_stamp = "",
                            amount2 = payment.Value.Amount,
                            amount_curr2 = amt_curr,
                            //BankOfEDC = "",
                            //AliBarcode = "",
                            //AliMerchantID = "",
                            //AliTransID = "",
                            //AlipayCancel = ""
                        };


                        _posDB.df_payment_onls.InsertOnSubmit(new_payment);
                    }

                    tran.Complete();

                    // order db save
                    var new_session_order = new order_session();
                    new_session_order.session_guid = Guid.NewGuid();


                    // return output order_session
                    order_session.SessionId = new_session_order.id;
                    order_session.SessionGuid = new_session_order.session_guid;

                }
                catch (Exception ex)
                {
                    tran.Dispose();
                    throw ex;
                }

            }

            return order_session;
        }

        public SaleAmountByPassport ValidateAllowSaleOnline(POSAirPortClassesDataContext _posDB, char terminal, string passort, DateTime date, int time)
        {
            var saleObj = _posDB.get_sale_passport_vol2(time, passort, date, terminal).FirstOrDefault();
            if (saleObj == null)
            {
                throw new System.ArgumentException("message", nameof(saleObj));
            }

            return new SaleAmountByPassport(saleObj);
        }

        public SaleOnlineByPassport VoidOrderOnline(string order_no)
        {
            throw new NotImplementedException();
        }
    }
}
