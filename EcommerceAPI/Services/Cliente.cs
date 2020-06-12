using EcomDataProccess;
using EcommerceAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAPI.Services
{
    public class Cliente
    {
        private readonly Ecommerce Ecommerce_;
        private readonly Ecom_Cliente ecom_Cliente;

        public Cliente()
        {
            try
            {
                Ecommerce_ = new Ecommerce();
                Ecommerce_.StartLib(LibraryEcommerce.Ecommerce);
                Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);
                ecom_Cliente = (Ecom_Cliente)Ecommerce_.ecomData.GetObject(ObjectSource.Cliente);
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
            finally
            {
                //Ecommerce_.ReleaseObjects();
            }

        }

        public Ecom_Cliente Get(int idCliente)
        {
            try
            {
                return ecom_Cliente.Get(idCliente) == false ? null : ecom_Cliente;
            }
            catch (Ecom_Exception ex)
            {
                return null;
            }
            finally
            {
                Ecommerce_.ReleaseObjects();
            }
        }

        public List<Ecom_Cliente> Get()
        {
            try
            {
                return ecom_Cliente.Get();
            }
            catch (Ecom_Exception ex)
            {
                return null;
            }
            finally
            {
                Ecommerce_.ReleaseObjects();
            }
        }
        public Ecom_Cliente Get(string user, string password)
        {
            try
            {
                return ecom_Cliente.Get(user,password);
            }
            catch (Ecom_Exception ex)
            {
                return null;
            }
            finally
            {
                Ecommerce_.ReleaseObjects();
            }
        }
    }
}
