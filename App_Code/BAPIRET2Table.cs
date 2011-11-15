
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

  public class BAPIRET2Table : SAPTable 
  {
    public static Type GetElementType() 
    {
        return (typeof(BAPIRET2));
    }
 
    public override object CreateNewRow()
    { 
        return new BAPIRET2();
    }
     
    public BAPIRET2 this[int index] 
    {
        get 
        {
            return ((BAPIRET2)(List[index]));
        }
        set 
        {
            List[index] = value;
        }
    }
        
    public int Add(BAPIRET2 value) 
    {
        return List.Add(value);
    }
        
    public void Insert(int index, BAPIRET2 value) 
    {
        List.Insert(index, value);
    }
        
    public int IndexOf(BAPIRET2 value) 
    {
        return List.IndexOf(value);
    }
        
    public bool Contains(BAPIRET2 value) 
    {
        return List.Contains(value);
    }
        
    public void Remove(BAPIRET2 value) 
    {
        List.Remove(value);
    }
        
    public void CopyTo(BAPIRET2[] array, int index) 
    {
        List.CopyTo(array, index);
	}
  }
}
