using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer.Dtos01.Export
{
    [XmlType("car")]
    public class BmwCarExportModel
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("model")]
        public string Model { get; set; }

        [XmlAttribute("travelled-distance")]
        public long TravelledDistance { get; set; }
    }

    //<car id = "7" model="1M Coupe" travelled-

    //                               distance="39826890" />

    //    <car id = "16" model="E67" travelled-distance="476830509"

    //    />
}
