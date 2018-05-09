using System;
using ProtoBuf;
namespace MatchingEngine.Modules.DataType
{
    [ProtoContract]
    public class Order{
        [ProtoMember(1)]
        public Int64 orderId;
        [ProtoMember(2)]
        public int type; //limited , market , stop gain ,stop loss
        [ProtoMember(3)]
        public bool isBid; //buy or sell
        [ProtoMember(4)]
        public int userId;
        [ProtoMember(5)]
        public string market;
        [ProtoMember(6)]
        public decimal price;
        [ProtoMember(7)]
        public decimal amount;
        [ProtoMember(8)]
        public decimal amountRemain;    
        [ProtoMember(8)]
        public decimal amountLastTimeRemain;       
        //not put fee in order , it control by other critier  
        //not put create time and update time as time is useless in matching engines
    }

}