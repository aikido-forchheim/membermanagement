using AVF.MemberManagement.StandardLibrary.Models.Tables;
using Newtonsoft.Json;
using Xunit;

namespace AVF.MemberManagement.xUnitIntegrationTests.StandardLibrary.Models.Tables
{
    public class TblTestsTest
    {
        [Fact]
        public void CanDeserialize()
        {
            const string sampleData = @"{ ""tblTest"": 
                                            [
                                                {
			                                        ""ID"": -1,
			                                        ""Text"": ""ok""
		                                        }, 
                                                {
			                                        ""ID"": 0,
			                                        ""Text"": ""ok""
		                                        }, 
                                                {
			                                        ""ID"": 1,
			                                        ""Text"": ""Ren\u00e9 M\u00fcller""
		                                        }, 
                                                {
			                                        ""ID"": 2,
			                                        ""Text"": ""hallo""
		                                        }
	                                        ]
                                        }";

            var testTableEntries = JsonConvert.DeserializeObject<TblTests>(sampleData).Rows;

            Assert.True(testTableEntries.Count > 0);
        }
    }
}
