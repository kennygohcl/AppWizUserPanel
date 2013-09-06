using Omu.Encrypto;
using dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.Core.Security;
using dFrontierAppWizard.Core.Service;
using dFrontierAppWizard.Infra;
using dFrontierAppWizard.Service;
using dFrontierAppWizard.WebUI.Mappers;
using dFrontierAppWizard.WebUI.Controllers;
using dFrontierAppWizard.WebUI.Dto;

namespace dFrontierAppWizard.WebUI
{
    public class WindsorConfigurator
    {
        public static void Configure()
        {
            WindsorRegistrar.Register(typeof(IFormsAuthentication), typeof(FormAuthService));
            WindsorRegistrar.Register(typeof(IHasher), typeof(Hasher));
            WindsorRegistrar.Register(typeof(IMapper<Promotion,PromotionInput>), typeof(PromotionMapper));
            WindsorRegistrar.Register(typeof(IUserService), typeof(UserService));
            WindsorRegistrar.Register(typeof(IProductService), typeof(ProductService));
            WindsorRegistrar.Register(typeof(IUserBillingInformationService), typeof(UserBillingInformationService));
            WindsorRegistrar.Register(typeof(IApplicationService), typeof(ApplicationService));
            WindsorRegistrar.Register(typeof(IConsumerBillingInformationService), typeof(ConsumerBillingInformationService));

            
            WindsorRegistrar.RegisterAllFromAssemblies("dFrontierAppWizard.Data");
            WindsorRegistrar.RegisterAllFromAssemblies("dFrontierAppWizard.Service");
            WindsorRegistrar.RegisterAllFromAssemblies("dFrontierAppWizard.WebUI");
        
        }
    }
}