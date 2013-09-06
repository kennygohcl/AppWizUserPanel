using System;
using dFrontierAppWizard.Core.Model;

namespace dFrontierAppWizard.Core.Service
{
    public interface IApplicationService : ICrudService<Application>
    {
        void SetMediaLink(int id, string root, string filename, int x, int y, int w, int h);
        string GetMediaValue(int id);
        Decimal GetTriggerPoint(int id);
    }
}