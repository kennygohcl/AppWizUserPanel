using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using NUnit.Framework;
using cm=dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.Core.Repository;
using dFrontierAppWizard.Data;
using dFrontierAppWizard.Infra;
using dFrontierAppWizard.WebUI.Mappers;
using dFrontierAppWizard.WebUI.Dto;
using Omu.ValueInjecter;

namespace dFrontierAppWizard.Tests
{
    public class ValueInjectionsTest
    {
        [Test]
        public void EntitiesToIntsTest()
        {
            var promotion = new cm.Promotion { Products = new List<cm.Product> { new cm.Product { Id = 3 }, new cm.Product { Id = 7 } } };

            var promotionInput = new PromotionInput();

            promotionInput.InjectFrom<EntitiesToInts>(promotion);

            Assert.IsNotNull(promotionInput.Products);
            Assert.AreEqual(2, promotionInput.Products.Count());
            Assert.AreEqual(3, promotionInput.Products.First());
        }

        [Test]
        public void IntsToEntities()
        {
            WindsorRegistrar.RegisterSingleton(typeof(IRepo<>), typeof(Repo<>));
            WindsorRegistrar.RegisterSingleton(typeof(IDbContextFactory), typeof(DbContextFactory));
            using (var scope = new TransactionScope())
            {
                var repo = new Repo<cm.Product>(new DbContextFactory());
                var product1 = new cm.Product { Name = "a" };
                var product2 = new cm.Product { Name = "b" };

                product1 = repo.Insert(product1);
                product2 = repo.Insert(product2);
                repo.Save();

                var promotionInput = new PromotionInput { Products = new List<int> { product1.Id, product2.Id } };
                var promotion = new cm.Promotion();

                promotion.InjectFrom<IntsToEntities>(promotionInput);

                Assert.IsNotNull(promotion.Products);
                Assert.AreEqual(2, promotion.Products.Count);
                Assert.AreEqual(product1.Id, promotion.Products.First().Id);
            }
        }

        [Test]
        public void NormalToNullables()
        {
            var promotion = new Promotion {};
            var promotionInput = new PromotionInput();

            promotionInput.InjectFrom<NormalToNullables>(promotion);

           
            Assert.AreEqual(null, promotionInput.Start);
           
        }

        [Test]
        public void NullablesToNormal()
        {
            var promotionInput = new PromotionInput {};
            var promotion = new cm.Promotion();

            promotion.InjectFrom<NullablesToNormal>(promotionInput);
            Assert.AreEqual(default(DateTime), promotion.Start);
        }
    }
}