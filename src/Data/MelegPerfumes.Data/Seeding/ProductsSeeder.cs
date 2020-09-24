using MelegPerfumes.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MelegPerfumes.Data.Seeding
{
    public class ProductsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var products = new List<(string Name, string Description, decimal Price, string ImageUrl, int ProductTypeId, int FragranceGroupId, ICollection<string> Notes, int YearOfManufacture)>
            {
                ("Ursa",
                  "The story of this fragrance originates around Lake Como. A body of water in which the changing moon and the Ursa Major lying above the high mountains were reflected.Ursa Major is an oriental fragrance with soft aromatic freshness.A burst of crisp, green elemi contrasts with nutmeg, dried fruits and rhum.This gives way to an intense, woody heart of patchouli, incense and tobacco blended with vetiver and olibanum.Softened by an unexpected base of vanilla with strong oud buaya, leather and agarwood, this contemporary scent comes to life with an invigorating, warm aroma.",
                  240.00M,
                  "https://res.cloudinary.com/pictures-storage/image/upload/v1587155007/product_images/jzj0b2dssfni4vqwsx4s.jpg",
                  dbContext.ProductTypes.SingleOrDefault(pt => pt.Name == "Fragrance").Id,
                  dbContext.FragranceGroups.SingleOrDefault(fg => fg.Name == "Oriental Spicy").Id,
                  new string[] { "Nutmeg", "Elemi", "Dried Fruits", "Rum", "Patchouli", "Olibanum", "Frankincense", "Tobacco absolute", "Vetiver", "Vanilla", "Leather accord", "Oud" },
                  2015),
                ("Porpora",
                "The inspiration behind this fragrance goes back to Paolo and Tiziana's childhood memories of time spent with their grandfather at their childhood house, watching Guglielmo pick the roses of their beautiful garden, and use them to make his precious absolutes, with wisdom and patience. It is on one of those nights that they saw a Super Moon, flaming and majestic, burst out from behind the flowery hillside. This creation was inspired by the magnificence of the famous Red Moon which sometimes leaps out, in a sudden fiery ball of light, from behind the horizon.",
                280.00M,
                "https://res.cloudinary.com/pictures-storage/image/upload/v1587154913/product_images/pk8iwucolkpqbwxn2tsi.jpg",
                dbContext.ProductTypes.SingleOrDefault(pt => pt.Name == "Fragrance").Id,
                dbContext.FragranceGroups.SingleOrDefault(fg => fg.Name == "Oriental Floral").Id,
                new string[] { "Rose", "Raspberry", "Cinnamon", "Cloves", "Bulgarian Rose", "Poppy", "Frankincense", "Patchouli", "Nutmeg", "Benzoin", "Amber", "Musk", "Myrrh" },
                2017),
                ("Caput Mundi",
                "For centuries the New Moon, that appears as a bright black circle, has been considered symbol of good luck and future prosperity; especially during the glorious era of the Roman Empire. Like every new beginning, even this time intervals defined by lunar cycles carried with them high hopes for the future and the possibility to make their wishes come true. This creation opens with a mellow and sweet flower note, where the precious Absolute of Bulgarian Rose echoes while paired to Rosa Tea and Lily of the Valley. This is when the invaluable Iris emerges, sustained by the strength of Cabreuva wood from Brazil.These regal and gentle head note lays on a brave heart created with Indian Black Oudh and Sandalwood, wisely blended with Red Patchouli. These woods find their strength thank to the echo of Orris(the most expensive ingredient of the perfumery) and Red Saffron from Sicily.This heart grows its roots on a base that is up to this regal wonder. It is only now that we can clearly sense the aphrodisiac push of Grey Amber, hugged by Cuban Cedar and Cashmere Wood.In the name of an extraordinary lasting we experince the imperial force of Vietnamese Agarwood and Cambodian Oudh. The everlasting strength of the City capital of the world enclosed in a drop of eternal regal beauty for true esthete of refined realities, with no time boundaries and without an end, just like the Black Moon cycle.",
                280.00M,
                "https://res.cloudinary.com/pictures-storage/image/upload/v1587154848/product_images/azjaq8oeeesmtwdg39s2.jpg",
                dbContext.ProductTypes.SingleOrDefault(pt => pt.Name == "Fragrance").Id,
                dbContext.FragranceGroups.SingleOrDefault(fg => fg.Name == "Woody").Id,
                new string[] { "Bulgarian Rose", "Iris", "Lily-of-the-valley", "Cabreuva", "Indian Oud", "Sandalwood", "Patchouli", "Saffron", "Orris", "Ambergris", "Cedar wood", "Cashmere Wood", "Oud", "Cambodian Oud" },
                2018),
                ("Eclix",
                "At the end of a warm, sun-kissed summer, Paolo and Tiziana encountered a Black Moon for the very first time. This particular eclipse terrified them, making them feel as though the darkness has swallowed their dear friend the Moon. Their grandfather immediately understood how they were feeling and held them in a warm and protective embrace. This unforgettable emotion was the spark that created Eclix, the fragrance of the Black Moon and of reassuring love. A new creation that joins the Luna Collection, where the fairy-tale element of memory explores the lines between magic and reality.",
                280.00M,
                "https://res.cloudinary.com/pictures-storage/image/upload/v1587155086/product_images/yfjznbcy6cobqrmixtzp.jpg",
                dbContext.ProductTypes.SingleOrDefault(pt => pt.Name == "Fragrance").Id,
                dbContext.FragranceGroups.SingleOrDefault(fg => fg.Name == "Floral Woody Musk").Id,
                new string[] { "Almond", "Iris", "Vanilla", "Tangerine", "Lemon", "Lemon", "Violet leaf", "White Lily", "Petitgrain", "Sandalwood", "Cedar wood", "Orris", "Musk keytone", "Amber" },
                2017),
            };

            foreach (var product in products)
            {
                if (!dbContext.Products.Any(p => p.Name == product.Name))
                {
                    await dbContext.Products.AddAsync(
                        new Product
                        {
                            Name = product.Name,
                            Description = product.Description,
                            Price = product.Price,
                            Picture = product.ImageUrl,
                            ProductTypeId = product.ProductTypeId,
                            FragranceGroupId = product.FragranceGroupId,
                            Notes = dbContext.Notes.Where(n => product.Notes.Contains(n.Name)).Select(n => new ProductNotes { ProductId = n.Id, NoteId = n.Id }).ToList(),
                            YearOfManufacture = product.YearOfManufacture,
                        });
                }
            }
        }
    }
}
