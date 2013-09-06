using cm=dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.WebUI.Dto;

namespace dFrontierAppWizard.WebUI.Mappers
{
    public class PromotionMapper : Mapper<cm.Promotion, PromotionInput>
    {
        public override cm.Promotion MapToEntity(PromotionInput input, cm.Promotion e)
        {
            var entity = base.MapToEntity(input, e);

            entity.Start = entity.Start;   //   entity.Start.AddHours(input.Hour).AddMinutes(input.Minute);
            entity.End = entity.End; //entity.Start.AddMinutes(input.Duration);
            
            return entity;
        }

        public override PromotionInput MapToInput(cm.Promotion entity)
        {
            var input = base.MapToInput(entity);

            input.Start = entity.Start;
            input.End = entity.End;
         /*   input.Minute = entity.Start.Minute;
            input.Hour = entity.Start.Hour;
            input.Duration = (int)entity.End.Subtract(entity.Start).TotalMinutes;
*/
            return input;
        }
    }
}