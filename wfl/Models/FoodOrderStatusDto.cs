using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Symphono.Wfl.Models
{
    public class FoodOrderStatusDto
    {
        public string Status { get; set; }
        public bool CanSetStatus(FoodOrder.StatusOptions currentStatus)
        {
            if (CanConvertStatusToEnum(currentStatus))
            {
                if (CanTransitionFromCurrentStatus(currentStatus))
                {
                    return true;
                }
            }
            return false;
        }
        private bool CanTransitionFromCurrentStatus(FoodOrder.StatusOptions currentStatus)
        {
            FoodOrder.StatusOptions status = (FoodOrder.StatusOptions) Enum.Parse(typeof(FoodOrder.StatusOptions), Status, false);
            if (currentStatus == status || currentStatus == FoodOrder.StatusOptions.Completed || (currentStatus == FoodOrder.StatusOptions.Discarded && status == FoodOrder.StatusOptions.Completed))
            {
                return false;
            }
            return true;
        }
        private bool CanConvertStatusToEnum(FoodOrder.StatusOptions currentStatus)
        {
            FoodOrder.StatusOptions status;
            if (Enum.TryParse(Status, false, out status))
            {
                return true;
            }
            return false;
        }

    }
}