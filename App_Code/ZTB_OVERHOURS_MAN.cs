
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

  [RfcStructure(AbapName ="ZTB_OVERHOURS_MAN" , Length = 20, Length2 = 36)]
  public class ZTB_OVERHOURS_MAN : SAPStructure
  {
    
    [RfcField(AbapName = "EMPLOYEE_ID", RfcType = RFCTYPE.RFCTYPE_NUM, Length = 8, Length2 = 16, Offset = 0, Offset2 = 0)]
    [XmlElement("EMPLOYEE_ID")]
    public string Employee_Id
    { 
       get
       {
          return _Employee_Id;
       }
       set
       {
          _Employee_Id = value;
       }
    }
    private string _Employee_Id;

    [RfcField(AbapName = "DATE_DAY", RfcType = RFCTYPE.RFCTYPE_DATE, Length = 8, Length2 = 16, Offset = 8, Offset2 = 16)]
    [XmlElement("DATE_DAY")]
    public string Date_Day
    { 
       get
       {
          return _Date_Day;
       }
       set
       {
          _Date_Day = value;
       }
    }
    private string _Date_Day;

    [RfcField(AbapName = "HOURS", RfcType = RFCTYPE.RFCTYPE_BCD, Length = 4, Length2 = 4, Decimals = 2, Offset = 16, Offset2 = 32)]
    [XmlElement("HOURS")]
    public Decimal Hours
    { 
       get
       {
          return _Hours;
       }
       set
       {
          _Hours = value;
       }
    }
    private Decimal _Hours;

  }

}
