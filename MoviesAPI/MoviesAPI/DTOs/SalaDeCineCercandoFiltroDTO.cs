﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.DTOs
{
    public class SalaDeCineCercandoFiltroDTO
    {
        [Range(-90, 90)]
        public double Latitud { get; set; }
        [Range(-180, 180)]
        public double Longitud { get; set; }
        public int distanciaEnKms = 10;
        public int distanciaMaximaKms = 50;
        public int DistanciaEnKms
        {
            get
            {
                return distanciaEnKms;
            }
            set 
            {
                distanciaEnKms = (value > distanciaMaximaKms) ? distanciaMaximaKms : value;
            }
        }

    }
}
