﻿using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using uMatrixCleaner.Xml;

namespace uMatrixCleaner
{
	public class UMatrixRule : IXmlSerializable, IEquatable<UMatrixRule>
	{
        public bool IsAllow { get; }

        public Selector Selector { get; }

        public int Priority => Selector.Specificity * 10 + (IsAllow ? 0 : 1);

        public UMatrixRule(HostPredicate source, HostPredicate destination, TypePredicate type, bool isAllow)
        {
            if (HostPredicate.N1stParty.Equals(source))
                throw new ArgumentException("source不能为1st-party。");

            Selector = new Selector(source, destination, type);
            IsAllow = isAllow;
        }


        public UMatrixRule(string line)
        {
            string[] parts = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            var source = new HostPredicate(parts[0]);
            var destination = new HostPredicate(parts[1]);
            var type = parts[2] == "*" ? uMatrixCleaner.TypePredicate.All : (TypePredicate)Enum.Parse(typeof(TypePredicate), parts[2], true);
            Selector = new Selector(source, destination, type);
            IsAllow = parts[3] == "allow";
        }

        public override bool Equals(object obj)
        {
            var other = obj as UMatrixRule;
            if (other == null)
                return false;

            return Equals(other);
        }

		public bool Equals(UMatrixRule obj)
		{
			var other = obj as UMatrixRule;
			if (other == null)
				return false;

            return Selector.Equals(other.Selector) && IsAllow == other.IsAllow;
        }

        public override int GetHashCode()
        {
            return Selector.Source.Value.GetHashCode() ^ Selector.Destination.Value.GetHashCode() ^ Selector.Type.GetHashCode() ^ IsAllow.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Selector} {(IsAllow ? "allow" : "block")}";
        }


        public string ToString(string separator)
        {
            return Selector.ToString(separator) + separator + (IsAllow ? "allow" : "block");
        }

		#region XML序列化

		// ReSharper disable once UnusedMember.Local
		private UMatrixRule() { }

		public XmlSchema GetSchema() { return null; }

		public void ReadXml(XmlReader reader)
		{
			reader.ReadToDescendant("Source");
			var source = reader.ReadElementContentAsString();
			var destination = reader.ReadElementContentAsString();
			var type = reader.ReadElementContentAsString();

			var selector = new Selector(new HostPredicate(source), new HostPredicate(destination), (TypePredicate)Enum.Parse(typeof(TypePredicate), type));
			this.SetGetterOnlyAutoProperty(nameof(Selector), selector);

			this.SetGetterOnlyAutoProperty(nameof(IsAllow), reader.ReadElementContentAsBoolean());
			reader.Read();
		}

		public void WriteXml(XmlWriter writer)
		{
			writer.WriteElementString("Source", Selector.Source.Value);
			writer.WriteElementString("Destination", Selector.Destination.Value);
			writer.WriteElementString("Type", Selector.Type.ToString());
			writer.WriteElementString("IsAllow", IsAllow.ToString().ToLowerInvariant());
		}

		#endregion
	}
}