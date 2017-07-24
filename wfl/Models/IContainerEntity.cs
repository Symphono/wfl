using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symphono.Wfl.Models
{
    interface IContainerEntity: IEntity
    {
        void OnDeserialize();
    }
}
