using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _2020.Utils;

namespace _2020.Solvers
{
    public class Day14Solver : ISolver
    {
        private readonly IDictionary<ulong, ulong> _memory = new Dictionary<ulong, ulong>();
        private readonly IDictionary<ulong, ulong> _v2Memory = new Dictionary<ulong, ulong>();
        private ulong? _mask;
        private ulong? _realBits;

        private string _maskString;
        private readonly HashSet<string> _addressXReplacements = new HashSet<string>();
        
        public void Solve(string input)
        {
            foreach (var line in input.Split(Environment.NewLine))
            {
                this.HandleLine(line);
            }

            var sum = this._memory.Values.Aggregate<ulong, ulong>(0, (current, v) => current + v);

            Console.WriteLine(sum);

            var v2Sum = this._v2Memory.Values.Aggregate<ulong, ulong>(0, (current, v) => current + v);

            Console.WriteLine(v2Sum);
        }

        private void HandleLine(string line)
        {
            var parts = line.Split(" = ");

            if (parts[0] == "mask")
            {
                this.ProcessMask(parts[1]);
            }
            else
            {
                if (!this._mask.HasValue || !this._realBits.HasValue)
                {
                    throw new ArgumentException("Tried to set memory without setting mask!");
                }
                
                var address = ulong.Parse(parts[0].Substring(4, parts[0].Length - 5));
                var rawValue = ulong.Parse(parts[1]);

                var valueBits = rawValue & this._realBits.Value;
                var realValue = valueBits | this._mask.Value;

                if (this._memory.ContainsKey(address))
                {
                    this._memory.Remove(address);
                }

                this._memory.Add(address, realValue);

                foreach (var floatingAddress in this.GetMemoryAddresses(ToBinaryString((int) address, this._maskString.Length)))
                {
                    if (this._v2Memory.ContainsKey(floatingAddress))
                    {
                        this._v2Memory.Remove(floatingAddress);
                    }
                    
                    this._v2Memory.Add(floatingAddress, rawValue);
                }
            }
        }

        private void ProcessMask(string mask)
        {
            this._maskString = mask;
            this._mask = Convert.ToUInt64(mask.Replace("X", "0"), 2);
            var realBits = new StringBuilder();

            foreach (var c in mask)
            {
                realBits.Append(c == 'X' ? '1' : '0');
            }

            this._realBits = Convert.ToUInt64(realBits.ToString(), 2);

            this._addressXReplacements.Clear();

            var numXs = mask.Count(c => c == 'X');
            var maxXNumber = Math.Pow(2, numXs);

            for (var i = 0; i < maxXNumber; i++)
            {
                var binaryString = ToBinaryString(i, numXs);
                this._addressXReplacements.Add(binaryString);
            }
        }

        private IEnumerable<ulong> GetMemoryAddresses(string address)
        {
            var addresses = new HashSet<ulong>();
            
            var newAddressBuilder = new StringBuilder();

            for (var i = 0; i < address.Length; i++)
            {
                newAddressBuilder.Append(this._maskString[i] == '0' ? address[i] : this._maskString[i]);
            }

            var newAddress = newAddressBuilder.ToString();

            foreach (var replacedAddress in this._addressXReplacements.Select(xReplacement => xReplacement.Aggregate(newAddress, (current, c) => current.ReplaceFirst('X', c))))
            {
                addresses.Add(Convert.ToUInt64(replacedAddress, 2));
            }
            
            return addresses;
        }

        private static string ToBinaryString(int number, int numBits)
        {
            var binaryString = new StringBuilder();

            while (numBits > 0)
            {
                if (number < 1)
                {
                    binaryString.Insert(0, 0);
                }
                else
                {
                    binaryString.Insert(0, number % 2);
                    number /= 2;    
                }

                numBits--;
            }

            return binaryString.ToString();
        }
    }
}