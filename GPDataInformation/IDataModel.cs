﻿using System;
using System.Collections.Generic;

namespace GPDataInformation
{
    public interface IDataModel<T>
    {

        bool Add();
        bool Update();
        bool Delete();
        int GetLastId();
        T Get(int? id);
        List<T> Get();
        void SetConnection(DBConnection dBConnection);
    }

    public enum IDataModelActions
    {
        Add = 1,
        Update = 2,
        delete = 3
    }
}
