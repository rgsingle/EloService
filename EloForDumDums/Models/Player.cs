﻿using System.ComponentModel.DataAnnotations;

namespace EloForDumDums.Models
{
    public class Player
    {
        [Key]
        public int UserId { get; set; }

        public int Wins { get; set; }

        public int Losses { get; set; }

        public double WinLossRatio { get; set; }

        public int Elo { get; set; }

        public int LongestWinstreak { get; set; }

        public int LongestLossstreak { get; set; }

        public int CurrentWinstreak { get; set; }

        public int CurrentLossstreak { get; set; }

        public double XpMultiplier { get; set; } = 1;
    }
}