using System;


namespace Portsmouth_Wiggle_console_app
{
    class Program
    {
        static void Main(string[] args)
        {
            Basket basket = new Basket();

            //product variables
            string response = "";
            string vouch = "";
            string PDescription = "";
            float PPrice = 0;
            int PQuantity = 0;
            string PCategory = "";
            bool PGiftV;

            //Voucher variables
            string voucherType;
            int offerVoucher = 0; //Number of offer vouchers being applied - limit is 1
            string voucherDescription = "";
            float voucherAmount = 0;
            float voucherThreshold = 0;
            string voucherCategory = "";

            do
            {
                Console.WriteLine("Would you like to add a product to the basket? Y/N");
                response = Console.ReadLine();

                if (!response.ToLower().Equals("y")) { break; }

                Console.WriteLine("Please enter the description of the product.");
                PDescription = Console.ReadLine();

                Console.WriteLine("Please enter the price of the product.");
                float.TryParse(Console.ReadLine(), out PPrice);

                Console.WriteLine("Please enter the quantity of the product.");
                int.TryParse(Console.ReadLine(), out PQuantity);

                Console.WriteLine("Is this product a gift card? true/false");
                bool.TryParse(Console.ReadLine(), out PGiftV);

                Console.WriteLine("Would you like to add a category for this product? Y/N");
                string cat = Console.ReadLine();

                if (cat.ToLower().Equals("y") ) // If the product has category
                {
                    Console.WriteLine("Please enter the category of the product.");
                    PCategory = Console.ReadLine();

                    //Add product with category to basket
                    Product product = new Product(PDescription, PPrice, PQuantity, PCategory, PGiftV);
                    basket.Products.Add(product);
                    Console.WriteLine("");
                    Console.WriteLine("Product added to the basket.");
                }
                else // Product has no category, asking to add another product
                {
                    //Add product without category to basket
                    Product product = new Product(PDescription, PPrice, PQuantity, PGiftV);
                    basket.Products.Add(product);
                    Console.WriteLine("");
                    Console.WriteLine("Product added to the basket.");
                }
            }
            while (response.ToLower().Equals("y") );

            //If doesn't want to add another product, ask to add voucher
            do
            {
                Console.WriteLine("Would you like to apply a voucher to the basket? Y/N");
                vouch = Console.ReadLine();

                if (!vouch.ToLower().Equals("y")) { break; }

                Console.WriteLine("What kind of voucher is this? Gift/Offer"); //Type of voucher
                voucherType = Console.ReadLine();

                if (voucherType.ToLower().Equals("offer") && offerVoucher < 1)
                //Offer voucher and less than 1 applied
                {
                    offerVoucher++;
                    Console.WriteLine("Please enter voucher's description. e.g. YYY-YYY ");
                    voucherDescription = Console.ReadLine();

                    Console.WriteLine("Please enter the amount the voucher will reduce. e.g. 5.00");
                    float.TryParse(Console.ReadLine(), out voucherAmount);

                    Console.WriteLine("Please enter the minimum threshold amount of the basked for the voucher to apply. e.g. 50.00");
                    float.TryParse(Console.ReadLine(), out voucherThreshold);

                    Console.WriteLine("Is this offer voucher limited to a certain category of products? Y/N");
                    voucherCategory = Console.ReadLine();

                    if (voucherCategory.ToLower().Equals("y")) //if voucher limited to category
                    {
                        Console.WriteLine("Please enter the category for which the offer voucher is limited to.");
                        voucherCategory = Console.ReadLine();

                        OfferVoucher voucher = new OfferVoucher(voucherDescription, voucherAmount, voucherThreshold, voucherCategory);
                        basket.Vouchers.Add(voucher);
                        Console.WriteLine("");
                        Console.WriteLine("Voucer added to the basket.");
                        continue;

                    } // if not limited to category, add offer voucher without category
                    else
                    {
                        OfferVoucher voucher = new OfferVoucher(voucherDescription, voucherAmount, voucherThreshold);
                        basket.Vouchers.Add(voucher);
                        Console.WriteLine("");
                        Console.WriteLine("Voucer added to the basket.");
                        continue;
                    }

                } //Offer voucher and less than 1 applied (if)
                else if (voucherType.ToString().ToLower().Equals("Offer".ToLower()) && offerVoucher > 0) //offer voucher and 1 already in basket
                {
                    Console.WriteLine("Only 1 Offer voucher allowed per basket");
                    continue;
                } // functionalityfor Gift voucher
                else if (voucherType.ToString().ToLower().Equals("Gift".ToLower() ) )
                {
                    Console.WriteLine("Please enter voucher's description. e.g. YYY-YYY ");
                    voucherDescription = Console.ReadLine();

                    Console.WriteLine("Please enter the amount the voucher will reduce. e.g. 5.00");
                    float.TryParse(Console.ReadLine(), out voucherAmount);
                   
                    basket.Vouchers.Add(new GiftVoucher(voucherDescription, voucherAmount));
                    Console.WriteLine("");
                    Console.WriteLine("Voucer added to the basket.");
                }
                



            } while (vouch.Equals("Y".ToLower())); // do while loop to apply voucher to basket

            // Doesn't/No longer wants to add voucher, start logic

            basket.displayProducts();


            Console.ReadLine();


        } // end of main
    }
}
