﻿using Repos.ViewModels.ProductVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos.IRepos
{
    public interface IProductRepo
    {
        GetProductsVM GetProduct();

    }
}
