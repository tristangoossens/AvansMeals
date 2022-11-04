using Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainServices.Services.Interfaces
{
    public interface IProductService
    {
        Product CreateNewProduct(
            string name,
            byte[] image,
            bool isAgeBound
        );
    }
}
