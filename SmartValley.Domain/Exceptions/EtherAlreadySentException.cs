﻿namespace SmartValley.Domain.Exceptions
{
    public class EtherAlreadySentException : AppErrorException
    {
        public EtherAlreadySentException(string address)
            : base(ErrorCode.EtherAlreadySent, $"Gift Ether has already been sent to the specified address: {address}")
        {
        }
    }
}