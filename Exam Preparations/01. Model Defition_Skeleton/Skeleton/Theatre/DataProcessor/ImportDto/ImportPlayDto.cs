using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;
using Theatre.Constraints;
using Theatre.Data.Models.Enums;

namespace Theatre.DataProcessor.ImportDto
{
    [XmlType("Play")]
    public class ImportPlayDto
    {
        [XmlElement("Title")]
        [Required]
        [MaxLength(GlobalConstraints.PLAY_TITLE_MAX_LENGTH)]
        [MinLength(GlobalConstraints.PLAY_TITLE_MIN_LENGTH)]
        public string Title { get; set; }

        [XmlElement("Duration")]
        [Required]
        public string Duration { get; set; }

        [XmlElement("Rating")]
        [Range(GlobalConstraints.PLAY_RATING_MIN, GlobalConstraints.PLAY_RATING_MAX)]
        public float Rating { get; set; }

        [Required]
        [XmlElement("Genre")]
        public string Genre { get; set; }

        [XmlElement("Description")]
        [MaxLength(GlobalConstraints.PLAY_DESCRIPTION_MAX_LENGTH)]
        [Required]
        public string Description { get; set; }

        [Required]
        [MaxLength(GlobalConstraints.PLAY_SCREEN_WRITER_MAX_LENGTH)]
        [MinLength((GlobalConstraints.PLAY_SCREEN_WRITER_MIN_LENGTH))]
        public string Screenwriter { get; set; }
    }
    //<Play>

    //    <Title>The Hsdfoming</Title>

    //    <Duration>03:40:00</Duration>

    //    <Rating>8.2</Rating>

    //    <Genre>Action</Genre>

    //    <Description>A guyat Pinter turns into a debatable conundrum as oth ordinary and

    //menacing. Much of this has to do with the fabled "Pinter Pause," which simply

    //mirrors the way we often respond to each other in conversation, tossing in

    //remainders of thoughts on one subject well after having moved on to

    //another.</Description>

    //    <Screenwriter>Roger Nciotti</Screenwriter>

    //    </Play>
}
