using DbFirst.ModelsDB;

namespace DbFirst;

public class Tests
{
    private DbfirstContext _context;
    [OneTimeSetUp]
    public void Setup()
    {
        _context = new DbfirstContext();
        Clear();
        Prepare();
    }

    [OneTimeTearDown]
    public void Teardown()
    {
        Clear();
    }

    private void Prepare()
    {
        var order = new Order()
        {
            OId = 1,
            OrderDate = DateOnly.FromDateTime(DateTime.Today)
        };

        var product = new Product()
        {
            PId = 1,
            PName = "Book",
            Price = 100
        };
        var product2 = new Product()
        {
            PId = 2,
            PName = "Book2",
            Price = 200
        };
        var product3 = new Product()
        {
            PId = 3,
            PName = "Book3",
            Price = 300
        };

        _context.Orders.Add(order);
        _context.Products.Add(product);
        _context.Products.Add(product2);
        _context.Products.Add(product3);

        _context.SaveChanges();
    }

    private void Clear()
    {
        _context.Orderitems.RemoveRange(_context.Orderitems.ToList());
        _context.Products.RemoveRange(_context.Products.ToList());
        _context.Orders.RemoveRange(_context.Orders.ToList());

        _context.SaveChanges();
    }

    [Test]
    public void Test1()
    {
        var order = _context.Orders.First(it => it.OId == 1);
        var product = _context.Products.First(it => it.PId == 1);
        Assert.That(order.OrderDate, Is.EqualTo(DateOnly.FromDateTime(DateTime.Today)));
        Assert.That(product.Price, Is.EqualTo(100));
    }

    [Test]
    public void Test2()
    {
        var order = _context.Orders.First(it => it.OId == 1);
        var product = _context.Products.First(it => it.PId == 1);
        var product2 = _context.Products.First(it => it.PId == 2);

        var orderItem = new Orderitem()
        {
            IId = 1,
            Order = order,
            Product = product,
            Amount = 10,
            Price = 100
        };
        _context.Orderitems.Add(orderItem);
        var orderItem2 = new Orderitem()
        {
            IId = 2,
            Order = order,
            Product = product2,
            Amount = 1,
            Price = 200
        };
        _context.Orderitems.Add(orderItem2);
        _context.SaveChanges();

        var dbOrderItem = _context.Orderitems.First(it => it.IId == 1);
        Assert.Multiple(() =>
        {
            Assert.That(dbOrderItem.OrderId, Is.EqualTo(1));
            Assert.That(dbOrderItem.ProductId, Is.EqualTo(1));
            Assert.That(dbOrderItem.Total, Is.EqualTo(1000));
        });

        var dbOrderItem2 = _context.Orderitems.First(it => it.IId == 2);
        Assert.Multiple(() =>
        {
            Assert.That(dbOrderItem2.OrderId, Is.EqualTo(1));
            Assert.That(dbOrderItem2.ProductId, Is.EqualTo(2));
            Assert.That(dbOrderItem2.Total, Is.EqualTo(200));
        });

        _context.SaveChanges();
    }
}