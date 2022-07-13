using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Theatre.Constraints;

namespace Theatre.DataProcessor.ImportDto
{
    public class ImportTicketDto
    {
        [Range(typeof(decimal), GlobalConstraints.TICKET_PRICE_MIN,
            GlobalConstraints.TICKET_PRICE_MAX)]
        public decimal Price { get; set; }

        [Range(typeof(sbyte), GlobalConstraints.TICKET_ROW_NUMBER_MIN, 
            GlobalConstraints.TICKET_ROW_NUMBER_MAX)]
        public sbyte RowNumber { get; set; }

        public int PlayId { get; set; }
    }
}
