
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

  public class ZTB_OVERHOURS_DIV_TYPE_TAB : SAPTable 
  {
    public static Type GetElementType() 
    {
        return (typeof(ZTB_OVERHOURS_DIV));
    }
 
    public override object CreateNewRow()
    { 
        return new ZTB_OVERHOURS_DIV();
    }
     
    public ZTB_OVERHOURS_DIV this[int index] 
    {
        get 
        {
            return ((ZTB_OVERHOURS_DIV)(List[index]));
        }
        set 
        {
            List[index] = value;
        }
    }
        
    public int Add(ZTB_OVERHOURS_DIV value) 
    {
        return List.Add(value);
    }
        
    public void Insert(int index, ZTB_OVERHOURS_DIV value) 
    {
        List.Insert(index, value);
    }
        
    public int IndexOf(ZTB_OVERHOURS_DIV value) 
    {
        return List.IndexOf(value);
    }
        
    public bool Contains(ZTB_OVERHOURS_DIV value) 
    {
        return List.Contains(value);
    }
        
    public void Remove(ZTB_OVERHOURS_DIV value) 
    {
        List.Remove(value);
    }
        
    public void CopyTo(ZTB_OVERHOURS_DIV[] array, int index) 
    {
        List.CopyTo(array, index);
	}
  }
}
