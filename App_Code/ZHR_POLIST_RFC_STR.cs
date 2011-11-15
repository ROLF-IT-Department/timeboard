
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

  [RfcStructure(AbapName ="ZHR_POLIST_RFC_STR" , Length = 32, Length2 = 64)]
  public class ZHR_POLIST_RFC_STR : SAPStructure
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

    [RfcField(AbapName = "START_PERIOD", RfcType = RFCTYPE.RFCTYPE_DATE, Length = 8, Length2 = 16, Offset = 8, Offset2 = 16)]
    [XmlElement("START_PERIOD")]
    public string Start_Period
    { 
       get
       {
          return _Start_Period;
       }
       set
       {
          _Start_Period = value;
       }
    }
    private string _Start_Period;

    [RfcField(AbapName = "BEGDA", RfcType = RFCTYPE.RFCTYPE_DATE, Length = 8, Length2 = 16, Offset = 16, Offset2 = 32)]
    [XmlElement("BEGDA")]
    public string Begda
    { 
       get
       {
          return _Begda;
       }
       set
       {
          _Begda = value;
       }
    }
    private string _Begda;

    [RfcField(AbapName = "ENDDA", RfcType = RFCTYPE.RFCTYPE_DATE, Length = 8, Length2 = 16, Offset = 24, Offset2 = 48)]
    [XmlElement("ENDDA")]
    public string Endda
    { 
       get
       {
          return _Endda;
       }
       set
       {
          _Endda = value;
       }
    }
    private string _Endda;

  }

}