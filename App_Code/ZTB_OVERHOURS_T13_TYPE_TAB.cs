
//------------------------------------------------------------------------------
// 
//     This code was generated by a SAP. NET Connector Proxy Generator Version 1.0
//     Created at 27.04.2009
//     Created from Windows 2000
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// 
//------------------------------------------------------------------------------
using System;
using System.Text;
using System.Collections;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using SAP.Connector;

namespace sap
{

  public class ZTB_OVERHOURS_T13_TYPE_TAB : SAPTable 
  {
    public static Type GetElementType() 
    {
        return (typeof(ZTB_OVERHOURS_T13));
    }
 
    public override object CreateNewRow()
    { 
        return new ZTB_OVERHOURS_T13();
    }
     
    public ZTB_OVERHOURS_T13 this[int index] 
    {
        get 
        {
            return ((ZTB_OVERHOURS_T13)(List[index]));
        }
        set 
        {
            List[index] = value;
        }
    }
        
    public int Add(ZTB_OVERHOURS_T13 value) 
    {
        return List.Add(value);
    }
        
    public void Insert(int index, ZTB_OVERHOURS_T13 value) 
    {
        List.Insert(index, value);
    }
        
    public int IndexOf(ZTB_OVERHOURS_T13 value) 
    {
        return List.IndexOf(value);
    }
        
    public bool Contains(ZTB_OVERHOURS_T13 value) 
    {
        return List.Contains(value);
    }
        
    public void Remove(ZTB_OVERHOURS_T13 value) 
    {
        List.Remove(value);
    }
        
    public void CopyTo(ZTB_OVERHOURS_T13[] array, int index) 
    {
        List.CopyTo(array, index);
	}
  }
}
