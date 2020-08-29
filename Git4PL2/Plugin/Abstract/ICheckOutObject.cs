using Git4PL2.Plugin.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.Abstract
{
    public interface ICheckOutObject :IDbObject, IEquatable<ICheckOutObject>
    {
        string ServerName { get; }
        string Login { get; }
        DateTime CheckoutDate { get; }
    }
}
