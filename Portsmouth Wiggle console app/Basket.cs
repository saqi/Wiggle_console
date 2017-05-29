using System;
using System.Collections.Generic;

public class Product
{
    //----------------------------------PROPERTIES---------------------------------
    public string Description { get; set; }
    public float Price { get; set; }
    public int Quantity { get; set; }
    public string Category { get; set; }
    public bool GiftVoucher { get; set; }


    //----------------------------------CONSTRUCTORS--------------------------------
    public Product(string description, float price, int quantity, string category, bool gvoucher)
    {
        this.Description = description;
        this.Price = price;
        this.Quantity = quantity;
        this.Category = category;
        this.GiftVoucher = gvoucher;
    }
    public Product(string description, float price, int quantity, bool gvoucher)
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

public class Voucher
{
    //----------------------------------PROPERTIES---------------------------------
    public string Description { get; set; }
    public float Amount { get; set; }

    //----------------------------------CONSTRUCTORS--------------------------------
    public Voucher()
    { }
    public Voucher(string description, float amount)
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
    public GiftVoucher(string description, float amount) : base(description, amount)
    { }
    //-------------------------------END OF CONSTRUCTORS--------------------------------

}

public class OfferVoucher : Voucher
{
    //· Have a threshold that needs to be matched or exceeded before a discount can be applied e.g. £5.00 off of baskets totalling £50
    //· Only a single offer voucher can be applied to a basket.
    //· Maybe applicable to only a subset of products.

    //-----------------------------PROPERTIES---------------------------------------------
    public float Threshold { get; set; }
    public string ProductCategory { get; set; }

    //----------------------------------CONSTRUCTORS--------------------------------
    public OfferVoucher() : base()
    { }
    public OfferVoucher(string description, float amount, float threshold) : base(description, amount)
    {
        this.Threshold = threshold;        
    }
    public OfferVoucher(string description, float amount, float threshold, string productCategory) : base(description, amount)
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
    public List<Voucher> Vouchers = new List<Voucher>();

    //----------------------------------CONSTRUCTORS--------------------------------
    public Basket()
    {
        this.Products = new List<Product>();
        this.Vouchers = new List<Voucher>();
    }
    public Basket(Product product)
    {
        this.Products.Add(product);
    }
    public Basket(Voucher voucher)
    {
        this.Vouchers.Add(voucher);
    }
    //-------------------------------END OF CONSTRUCTORS--------------------------------

    // basket total price functionality
    public Tuple<float, float, float> totalPrice()
    {
        float total = 0; float giftVoucherTotal = 0; float giftVoucherDeductedTotal = 0;
        for (int count = 0; count < Products.Count; count++)
        {
            if ( this.Products[count].GiftVoucher == true ) {
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




}
