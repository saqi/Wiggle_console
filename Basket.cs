using System;

public class Basket
{
    public class Product
    {
        float total = 0;

        public Product (string description, string price, int quantity, string category)
        {
            this.Description = description;
            this.Price = price;
            this.Quantity = quantity;
            this.Category = category;
        }
    }

	
}
