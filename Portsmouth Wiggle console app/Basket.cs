using System;
using System.Collections.Generic;

public class Product
{
    //----------------------------------PROPERTIES---------------------------------
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string Category { get; set; }
    public bool GiftVoucher { get; set; }


    //----------------------------------CONSTRUCTORS--------------------------------
    public Product(string description, decimal price, int quantity, string category, bool gvoucher)
    {
        this.Description = description;
        this.Price = price;
        this.Quantity = quantity;
        this.Category = category;
        this.GiftVoucher = gvoucher;
    }
    public Product(string description, decimal price, int quantity, bool gvoucher)
    {
        this.Description = description;
        this.Price = price;
        this.Quantity = quantity;
        this.Category = "";
        this.GiftVoucher = gvoucher;
    }
    public Product()
    {
        this.Description = "";
        this.Price = 0;
        this.Quantity = 0;
        this.Category = "";
        this.GiftVoucher = false;
    }
    //-------------------------------END OF CONSTRUCTORS--------------------------------



} // END OF PRODUCT CLASS

public abstract class Voucher
{
    //----------------------------------PROPERTIES---------------------------------
    public string Description { get; set; }
    public decimal Amount { get; set; }

    //----------------------------------CONSTRUCTORS--------------------------------
    public Voucher()
    { }
    public Voucher(string description, decimal amount)
    {
        this.Description = description;
        this.Amount = amount;
    }
    //-------------------------------END OF CONSTRUCTORS--------------------------------
}

public class GiftVoucher : Voucher
{
    //· Can be redeemed against the value of a basket.
    //· Multiple gift vouchers can be applied to a basket.

    //----------------------------------CONSTRUCTORS--------------------------------
    public GiftVoucher() : base()
    { }
    public GiftVoucher(string description, decimal amount) : base(description, amount)
    { }
    //-------------------------------END OF CONSTRUCTORS--------------------------------

}

public class OfferVoucher : Voucher
{
    //· Have a threshold that needs to be matched or exceeded before a discount can be applied e.g. £5.00 off of baskets totalling £50
    //· Only a single offer voucher can be applied to a basket.
    //· Maybe applicable to only a subset of products.

    //-----------------------------PROPERTIES---------------------------------------------
    public decimal Threshold { get; set; }
    public string ProductCategory { get; set; }

    //----------------------------------CONSTRUCTORS--------------------------------
    public OfferVoucher() : base()
    { }
    public OfferVoucher(string description, decimal amount, decimal threshold) : base(description, amount)
    {
        this.Threshold = threshold;
    }
    public OfferVoucher(string description, decimal amount, decimal threshold, string productCategory) : base(description, amount)
    {
        this.Threshold = threshold;
        this.ProductCategory = productCategory;
    }
    //-------------------------------END OF CONSTRUCTORS--------------------------------
}

public class Basket
{
    //-----------------------------PROPERTIES---------------------------------------------
    public List<Product> Products = new List<Product>();
    public List<OfferVoucher> offerVouchers = new List<OfferVoucher>();
    public List<GiftVoucher> giftVouchers = new List<GiftVoucher>();

    //----------------------------------CONSTRUCTORS--------------------------------
    public Basket()
    {
        this.Products = new List<Product>();
        this.offerVouchers = new List<OfferVoucher>();
        this.giftVouchers = new List<GiftVoucher>();
    }
    public Basket(Product product)
    {
        this.Products.Add(product);
    }
    public Basket(OfferVoucher voucher)
    {
        this.offerVouchers.Add(voucher);
    }
    public Basket(GiftVoucher voucher)
    {
        this.giftVouchers.Add(voucher);
    }
    //-------------------------------END OF CONSTRUCTORS--------------------------------

    //------------------------------------------------------------------FUNCTIONS-----------------------------------------------------------------------

    // basket total price functionality
    public Tuple<decimal, decimal, decimal> totalPrice(decimal offerVoucherDiscount = 0, decimal GiftVoucherDiscount = 0)
    {
        decimal total = 0; decimal giftVoucherTotal = 0; decimal giftVoucherDeductedTotal = 0;
        for (int count = 0; count < Products.Count; count++)
        {
            if (this.Products[count].GiftVoucher == true)
            {
                giftVoucherTotal += this.Products[count].Price;
            }
            total += this.Products[count].Price * this.Products[count].Quantity;
        }
        giftVoucherDeductedTotal = total - giftVoucherTotal;
        return Tuple.Create(total, giftVoucherTotal, giftVoucherDeductedTotal);
    }


    //Console WL display products
    public void displayProducts()
    {
        for (int i = 0; i < this.Products.Count; i++)
        {
            string cat = this.Products[i].Category.Equals("") ? "" : " (" + this.Products[i].Category +
                " Category of Product)";

            Console.WriteLine(this.Products[i].Quantity + " " + this.Products[i].Description + cat +
                " @ £" + this.Products[i].Price
                );
        }
        Console.WriteLine("------------");
        // Console.WriteLine("Total:£" + this.totalPrice()); // Displaying total price of products
    }

    //---------------------------------------OFFER VOUCHER CHECKS---------------------------------------
    //Check whether there are offer vouchers
    public bool offerVoucherExists()
    {
        if (offerVouchers.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    } //Check whether there are offer vouchers

    //Display vouchers
    public void displayVouchers()
    {
        foreach (var voucher in offerVouchers)
        {
            string VCATEGORY = voucher.ProductCategory != null ? voucher.ProductCategory : "";

            Console.WriteLine("1 x £" + voucher.Amount + " off " + VCATEGORY + " in baskets over £" + voucher.Threshold + " Offer Voucher " + voucher.Description + " applied");
        }

        foreach (var voucher in giftVouchers)
        {
            Console.WriteLine("1 x £" + voucher.Amount + " Gift Voucher " + voucher.Description + " applied");
        }

        Console.WriteLine("------------");
    }

    //check if offer voucher category applied exists in products
    public bool offerVoucherCategoryMatches()
    {
        if (offerVoucherExists())
        {
            string offerCategory = null;

            for (int i = 0; i < this.offerVouchers.Count; i++) // Assign category of voucher even though there is only 1 voucher (for future)
            {
                if (this.offerVouchers[i].ProductCategory != null)
                {
                    offerCategory = this.offerVouchers[i].ProductCategory;
                }
            }

            for (int i = 0; i < this.Products.Count; i++)
            {
                if (!this.Products[i].Category.Equals("")) //if product category is not null
                {
                    if (this.Products[i].Category.Equals(offerCategory))
                    {
                        return true;
                    }
                }
                else // There are no products which the offer voucher can be applied to as categories don't match
                {
                    return false;
                }
            }

        } //if offer voucher exists 

        return false;


    } //end of function: check if offer voucher category applied exists in products returns true or false


    // function: offer voucher discount
    public Tuple<decimal, string> offerVoucherDiscount(decimal discount = 0, string message = "")
    {
        //string message = "";

        if (offerVoucherExists())
        {
            if (this.offerVouchers[0].ProductCategory == null) //No category just threshold 
            {
                if (totalPrice().Item3 > this.offerVouchers[0].Threshold)
                {
                    //return discount amount
                    return Tuple.Create(this.offerVouchers[0].Amount, message);
                }
                else
                {
                    message += "Add £" + (offerVouchers[0].Threshold - totalPrice().Item3) + " value to the basket.\n";
                    return Tuple.Create(Convert.ToDecimal(0), message);
                }
            } // //No category just threshold , now if category
            else
            { // //category, check for threshold & matching
                if (totalPrice().Item3 < offerVouchers[0].Threshold) // Threshold not met
                {
                    message += "Add £" + Convert.ToString((offerVouchers[0].Threshold - totalPrice().Item3)) + " value to the basket.\n";
                    return Tuple.Create(Convert.ToDecimal(0), message);
                }
                else if (totalPrice().Item3 > offerVouchers[0].Threshold && offerVoucherCategoryMatches()) //threshold met and basket has appropriate product for voucher
                {
                    //return discount amount
                    decimal categoryTotals = 0, discountToApply = 0;
                    foreach (var product in Products)
                    {
                        if (!product.Category.Equals("") && product.Category.Equals(this.offerVouchers[0].ProductCategory))
                            categoryTotals += product.Price;
                    }
                    //only apply discount 
                    discount = categoryTotals - this.offerVouchers[0].Amount;
                    if (discount < 0)
                    {
                        return Tuple.Create(Convert.ToDecimal(0), message);
                    }
                    else
                    {
                        return Tuple.Create(Convert.ToDecimal(discountToApply), message);
                    }
                }
                else
                {
                    message += "There are no products in your basket applicable to voucher " + this.offerVouchers[0].Description + ".\n";
                    return Tuple.Create(Convert.ToDecimal(0), message);
                }
            }

        }
        //Voucher doesn't exist so 0 from Offer Voucher
        return Tuple.Create(Convert.ToDecimal(0), message);

    } // end of function: offer voucher discount

    //function: gift voucher discount
    public decimal giftVoucherDiscount(decimal discount = 0)
    {
        if (giftVouchers.Count > 0)
        {
            foreach (var voucher in giftVouchers)
            {
                discount += voucher.Amount;
            }
            return discount;
        }

        return 0;

    }

    //Apply discount on basket, if total < 0 return 0
    public Tuple<decimal, string> applyDiscounts(decimal offerVDiscount = 0, decimal giftVDiscount = 0, string message = "")
    {
        var totalDiscount = offerVDiscount + giftVDiscount;

        if (totalDiscount > totalPrice().Item3)
        {
            totalDiscount = totalPrice().Item3;
            Console.WriteLine("Total: £" + (totalPrice().Item1 - totalDiscount));
            Console.WriteLine(message);
            return Tuple.Create(totalPrice().Item1 - totalDiscount, message);

        }

        Console.WriteLine("Total: £" + (totalPrice().Item1 - totalDiscount));

        return Tuple.Create(totalPrice().Item1 - totalDiscount, message);

    }


}
