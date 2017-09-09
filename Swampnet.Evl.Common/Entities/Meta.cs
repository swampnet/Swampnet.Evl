using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl.Common.Entities
{
	public class MetaData
	{
		public ActionMetaData[] ActionMetaData { get; set; }
		public ExpressionOperator[] Operators { get; set; }
		public MetaDataCapture[] Operands { get; set; }

        public static MetaData Default => _meta;

        // Hard coding all this, but should really generate it all dynamically.
        private static readonly MetaData _meta = new MetaData()
        {
            ActionMetaData = new[]
            {
                new ActionMetaData()
                {
                    Type = "email",
                    Properties = new[]
                    {
                        new MetaDataCapture()
                        {
                            Name = "to",
                            IsRequired = true,
                            Options = new[]
                            {
                                new Option(null, ".*@.*")
                            }
                        },
                        new MetaDataCapture()
                        {
                            Name = "cc",
                            IsRequired = false,
                            Options = new[]
                            {
                                new Option(null, ".*@.*")
                            }
                        },
                        new MetaDataCapture()
                        {
                            Name = "bcc",
                            IsRequired = false,
                            Options = new[]
                            {
                                new Option(null, ".*@.*")
                            }
                        },
                    }
                },

				new ActionMetaData()
				{
					Type = "slack",
					Properties = new[]
					{
						new MetaDataCapture()
						{
							Name = "channel",
							IsRequired = true
						}
					}
				},

				new ActionMetaData()
                {
                    Type = "change-category",
                    Properties = new[]
                    {
                        new MetaDataCapture()
                        {
                            Name = "Category",
                            IsRequired = true,
                            DataType = "select",
                            Options = new[]
                            {
                                new Option("Information", "Information"),
                                new Option("Error", "Error"),
                                new Option("Debug", "Debug")
                            }
                        }
                    }
                },

                new ActionMetaData()
                {
                    Type = "add-property",
                    Properties = new[]
                    {
                        new MetaDataCapture()
                        {
                            Name = "category",
                            IsRequired = false,
                        },
                        new MetaDataCapture()
                        {
                            Name = "name",
                            IsRequired = true,
                        },
                        new MetaDataCapture()
                        {
                            Name = "value",
                            IsRequired = true,
                        }
                    }
                }
            },

            Operators = new[]
            {
                new ExpressionOperator(RuleOperatorType.MATCH_ALL, "Match All", true),
                new ExpressionOperator(RuleOperatorType.MATCH_ANY, "Match Any", true),

                new ExpressionOperator(RuleOperatorType.EQ, "=", false),
                new ExpressionOperator(RuleOperatorType.NOT_EQ, "<>", false),
                new ExpressionOperator(RuleOperatorType.GT, ">", false),
                new ExpressionOperator(RuleOperatorType.GTE, ">=", false),
                new ExpressionOperator(RuleOperatorType.LT, "<", false),
                new ExpressionOperator(RuleOperatorType.LTE, "<=", false),
                new ExpressionOperator(RuleOperatorType.REGEX, "Match Expression", false)
            },

            Operands = new[]
            {
                new MetaDataCapture()
                {
                    Name = "Summary"
                },

                new MetaDataCapture()
                {
                    Name = "Timestamp",
                    DataType = "datetime"
                },

                new MetaDataCapture()
                {
                    Name = "Category",
                    DataType = "select",
                    Options = new[]
                    {
                        new Option("Information", "Information"),
                        new Option("Error", "Error"),
                        new Option("Debug", "Debug")
                    }
                },

                new MetaDataCapture()
                {
                    Name = "Property",
                    DataType = "require-args"
                }
            }
        };

    }


    public class Option
	{
		public Option()
		{
		}

		public Option(string display, string value)
			: this()
		{
			Display = display;
			Value = value;
		}

		public string Display { get; set; }
		public string Value { get; set; }
	}


	public class ActionMetaData
	{
		public string Type { get; set; }
		public MetaDataCapture[] Properties { get; set; }
	}



	public class ExpressionOperator
	{
		public ExpressionOperator()
		{
		}

		public ExpressionOperator(RuleOperatorType op, string display, bool isGroup)
			: this()
		{
			Code = op;
			Display = display;
			IsGroup = isGroup;
		}


		[JsonConverter(typeof(StringEnumConverter))]
		public RuleOperatorType Code { get; set; }
		public string Display { get; set; }
		public bool IsGroup { get; set; }
	}

	public class MetaDataCapture
	{
		public string Name { get; set; }
		public bool IsRequired { get; set; }
		public string DataType { get; set; }
		public Option[] Options { get; set; }
	}
}
