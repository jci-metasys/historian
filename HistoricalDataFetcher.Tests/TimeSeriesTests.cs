using HistoricalDataFetcher.Classes.DataLayer.Cache;
using HistoricalDataFetcher.Classes.Endpoints;
using HistoricalDataFetcher.Classes.Models.Collection;
using HistoricalDataFetcher.Classes.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace HistoricalDataFetcher.Tests
{
    [TestClass]
    public class TimeSeriesTests
    {
        private static TimeSeriesEndPoint _timeSeriesEndPoint;

        [TestInitialize]
        public void TimeSeriesTestsInit()
        {
            var timeSeriesSetup = new Mock<TimeSeriesEndPoint>(null) { CallBase = true };

            timeSeriesSetup.Setup(x =>
                    x.GetCollectionAsync<SampleCollectionItem>(It.Is<string>(y => y.ToLower().Contains("/objects/11111111-1111-1111-1111-111111111111/attributes/85/samples".ToLower()))))
                .Returns(SamplesMoqHelper("11111111-1111-1111-1111-111111111111"));

            timeSeriesSetup.Setup(x =>
                    x.GetCollectionAsync<SampleCollectionItem>(It.Is<string>(y => y.ToLower().Contains("/objects/22222222-2222-2222-2222-222222222222/attributes/85/samples".ToLower()))))
                .Returns(SamplesMoqHelper("22222222-2222-2222-2222-222222222222"));

            timeSeriesSetup.Setup(x =>
                    x.GetCollectionAsync<SampleCollectionItem>(It.Is<string>(y => y.ToLower().Contains("/objects/33333333-3333-3333-3333-333333333333/attributes/85/samples".ToLower()))))
                .Returns(SamplesMoqHelper("33333333-3333-3333-3333-333333333333"));

            timeSeriesSetup.Setup(x => x.SaveDataAsync()).Returns(Task.FromResult(true));
            _timeSeriesEndPoint = timeSeriesSetup.Object;

            var cacheMock = new Mock<ICache>();
            ApiRequest.InitializeAsync(cacheMock.Object, "test", "password", "localhost", true).Wait();
        }

        [TestMethod]
        public void TimeSeriesDataSinglePointTest()
        {
            var result = _timeSeriesEndPoint.RunAsync("/objects/11111111-1111-1111-1111-111111111111/attributes/85/samples").Result;
            Assert.IsTrue(result);
            Assert.AreEqual(3, _timeSeriesEndPoint.SampleDataCount);
        }

        [TestMethod]
        public void TimeSeriesDataListOfPointsTest()
        {
            string[] pointUrlArray = new[]
            {
                "/objects/11111111-1111-1111-1111-111111111111/attributes/85/samples",
                "/objects/22222222-2222-2222-2222-222222222222/attributes/85/samples",
                "/objects/33333333-3333-3333-3333-333333333333/attributes/85/samples"
            };
            int totalCount = 0;

            foreach (var url in pointUrlArray)
            {
                var result = _timeSeriesEndPoint.RunAsync(url).Result;
                Assert.IsTrue(result);
                totalCount += _timeSeriesEndPoint.SampleDataCount;
            }

            Assert.AreEqual(45, totalCount);
        }

        [TestMethod]
        public void TimeSeriesDataSinglePointNoSamplesTest()
        {
            var result = _timeSeriesEndPoint.RunAsync("/objects/44444444-4444-4444-4444-444444444444/attributes/85/samples").Result;
            Assert.IsFalse(result);
            Assert.AreEqual(0, _timeSeriesEndPoint.SampleDataCount);
        }


        private static Task<BatchCollection<SampleCollectionItem>> SamplesMoqHelper(string guid)
        {
            string samples = string.Empty;

            if (guid.Equals("11111111-1111-1111-1111-111111111111"))
            {
                samples = @"{
                    ""total"": 3,
                    ""next"": null,
                    ""previous"": null,
                    ""items"": [
                        {
                            ""value"": {
                                ""value"": 0.0,
                                ""units"": ""https://localhost/API/api/v1/enumSets/6/members/0""
                            },
                            ""timestamp"": ""2018-06-30T23:06:00.0000000Z"",
                            ""isReliable"": false
                        },
                        {
                            ""value"": {
                                ""value"": 0.0,
                                ""units"": ""https://localhost/API/api/v1/enumSets/6/members/0""
                            },
                            ""timestamp"": ""2018-07-01T00:46:00.0000000Z"",
                            ""isReliable"": false
                        },
                        {
                            ""value"": {
                                ""value"": 0.0,
                                ""units"": ""https://localhost/API/api/v1/enumSets/6/members/0""
                            },
                            ""timestamp"": ""2018-07-01T02:26:00.0000000Z"",
                            ""isReliable"": false
                        }
                    ],
                    ""attribute"": ""https://localhost/API/api/v1/enumSets/509/members/85"",
                    ""self"": ""https://localhost/API.TimeSeriesService/api/v1/points/11111111-1111-1111-1111-111111111111/samples/85?startTime={DateTime.Now.AddDays(-1)}&endTime={DateTime.Now}&page=1&pageSize=1000&sort=timestamp"",
                    ""point"": ""https://localhost/API/api/v1/points/11111111-1111-1111-1111-111111111111""
                }";
            }
            else if (guid.Equals("22222222-2222-2222-2222-222222222222"))
            {
                samples = @"{
                    ""total"": 1,
                    ""next"": null,
                    ""previous"": null,
                    ""items"": [
                        {
                            ""value"": {
                                ""value"": 0.0,
                                ""units"": ""https://localhost/API/api/v1/enumSets/6/members/0""
                            },
                            ""timestamp"": ""2018-06-30T23:06:00.0000000Z"",
                            ""isReliable"": false
                        },
                    ],
                    ""attribute"": ""https://localhost/API/api/v1/enumSets/509/members/85"",
                    ""self"": ""https://localhost/API.TimeSeriesService/api/v1/points/22222222-2222-2222-2222-222222222222/samples/85?startTime={DateTime.Now.AddDays(-1)}&endTime={DateTime.Now}&page=1&pageSize=1000&sort=timestamp"",
                    ""point"": ""https://localhost/API/api/v1/points/22222222-2222-2222-2222-222222222222""
                }";
            }
            else if (guid.Equals("33333333-3333-3333-3333-333333333333"))
            {
                samples = @"{
                ""total"": 41,
                ""next"": null,
                ""previous"": null,
                ""items"": [
                    {
                        ""value"": {
                            ""value"": 0.0,
                            ""units"": ""https://localhost/API/api/v1/enumSets/6/members/0""
                        },
                        ""timestamp"": ""2018-06-30T23:06:00.0000000Z"",
                        ""isReliable"": false
                    },
                    {
                        ""value"": {
                            ""value"": 0.0,
                            ""units"": ""https://localhost/API/api/v1/enumSets/6/members/0""
                        },
                        ""timestamp"": ""2018-07-01T00:46:00.0000000Z"",
                        ""isReliable"": false
                    },
                    {
                        ""value"": {
                            ""value"": 0.0,
                            ""units"": ""https://localhost/API/api/v1/enumSets/6/members/0""
                        },
                        ""timestamp"": ""2018-07-01T02:26:00.0000000Z"",
                        ""isReliable"": false
                    },
                    {
                        ""value"": {
                            ""value"": 0.0,
                            ""units"": ""https://localhost/API/api/v1/enumSets/6/members/0""
                        },
                        ""timestamp"": ""2018-07-01T04:06:00.0000000Z"",
                        ""isReliable"": false
                    },
                    {
                        ""value"": {
                            ""value"": 0.0,
                            ""units"": ""https://localhost/API/api/v1/enumSets/6/members/0""
                        },
                        ""timestamp"": ""2018-07-01T05:46:00.0000000Z"",
                        ""isReliable"": false
                    },
                    {
                        ""value"": {
                            ""value"": 0.0,
                            ""units"": ""https://localhost/API/api/v1/enumSets/6/members/0""
                        },
                        ""timestamp"": ""2018-07-01T07:26:00.0000000Z"",
                        ""isReliable"": false
                    },
                    {
                        ""value"": {
                            ""value"": 0.0,
                            ""units"": ""https://localhost/API/api/v1/enumSets/6/members/0""
                        },
                        ""timestamp"": ""2018-07-01T09:06:00.0000000Z"",
                        ""isReliable"": false
                    },
                    {
                        ""value"": {
                            ""value"": 0.0,
                            ""units"": ""https://localhost/API/api/v1/enumSets/6/members/0""
                        },
                        ""timestamp"": ""2018-07-01T10:46:00.0000000Z"",
                        ""isReliable"": false
                    },
                    {
                        ""value"": {
                            ""value"": 0.0,
                            ""units"": ""https://localhost/API/api/v1/enumSets/6/members/0""
                        },
                        ""timestamp"": ""2018-07-01T12:26:00.0000000Z"",
                        ""isReliable"": false
                    },
                    {
                        ""value"": {
                            ""value"": 0.0,
                            ""units"": ""https://localhost/API/api/v1/enumSets/6/members/0""
                        },
                        ""timestamp"": ""2018-07-01T14:06:00.0000000Z"",
                        ""isReliable"": false
                    },
                    {
                        ""value"": {
                            ""value"": 0.0,
                            ""units"": ""https://localhost/API/api/v1/enumSets/6/members/0""
                        },
                        ""timestamp"": ""2018-07-01T15:46:00.0000000Z"",
                        ""isReliable"": false
                    },
                    {
                        ""value"": {
                            ""value"": 0.0,
                            ""units"": ""https://localhost/API/api/v1/enumSets/6/members/0""
                        },
                        ""timestamp"": ""2018-07-01T17:26:00.0000000Z"",
                        ""isReliable"": false
                    },
                    {
                        ""value"": {
                            ""value"": 0.0,
                            ""units"": ""https://localhost/API/api/v1/enumSets/6/members/0""
                        },
                        ""timestamp"": ""2018-07-01T19:06:00.0000000Z"",
                        ""isReliable"": false
                    },
                    {
                        ""value"": {
                            ""value"": 0.0,
                            ""units"": ""https://localhost/API/api/v1/enumSets/6/members/0""
                        },
                        ""timestamp"": ""2018-07-01T20:46:00.0000000Z"",
                        ""isReliable"": false
                    },
                    {
                        ""value"": {
                            ""value"": 0.0,
                            ""units"": ""https://localhost/API/api/v1/enumSets/6/members/0""
                        },
                        ""timestamp"": ""2018-07-01T22:26:00.0000000Z"",
                        ""isReliable"": false
                    },
                    {
                        ""value"": {
                            ""value"": 0.0,
                            ""units"": ""https://localhost/API/api/v1/enumSets/6/members/0""
                        },
                        ""timestamp"": ""2018-07-02T00:06:00.0000000Z"",
                        ""isReliable"": false
                    },
                    {
                        ""value"": {
                            ""value"": 0.0,
                            ""units"": ""https://localhost/API/api/v1/enumSets/6/members/0""
                        },
                        ""timestamp"": ""2018-07-02T01:46:00.0000000Z"",
                        ""isReliable"": false
                    },
                    {
                        ""value"": {
                            ""value"": 0.0,
                            ""units"": ""https://localhost/API/api/v1/enumSets/6/members/0""
                        },
                        ""timestamp"": ""2018-07-02T03:26:00.0000000Z"",
                        ""isReliable"": false
                    },
                    {
                        ""value"": {
                            ""value"": 0.0,
                            ""units"": ""https://localhost/API/api/v1/enumSets/6/members/0""
                        },
                        ""timestamp"": ""2018-07-02T05:06:00.0000000Z"",
                        ""isReliable"": false
                    },
                    {
                        ""value"": {
                            ""value"": 0.0,
                            ""units"": ""https://localhost/API/api/v1/enumSets/6/members/0""
                        },
                        ""timestamp"": ""2018-07-02T06:46:00.0000000Z"",
                        ""isReliable"": false
                    },
                    {
                        ""value"": {
                            ""value"": 0.0,
                            ""units"": ""https://localhost/API/api/v1/enumSets/6/members/0""
                        },
                        ""timestamp"": ""2018-07-02T08:26:00.0000000Z"",
                        ""isReliable"": false
                    },
                    {
                        ""value"": {
                            ""value"": 0.0,
                            ""units"": ""https://localhost/API/api/v1/enumSets/6/members/0""
                        },
                        ""timestamp"": ""2018-07-02T10:06:00.0000000Z"",
                        ""isReliable"": false
                    },
                    {
                        ""value"": {
                            ""value"": 0.0,
                            ""units"": ""https://localhost/API/api/v1/enumSets/6/members/0""
                        },
                        ""timestamp"": ""2018-07-02T11:46:00.0000000Z"",
                        ""isReliable"": false
                    },
                    {
                        ""value"": {
                            ""value"": 0.0,
                            ""units"": ""https://localhost/API/api/v1/enumSets/6/members/0""
                        },
                        ""timestamp"": ""2018-07-02T13:26:00.0000000Z"",
                        ""isReliable"": false
                    },
                    {
                        ""value"": {
                            ""value"": 0.0,
                            ""units"": ""https://localhost/API/api/v1/enumSets/6/members/0""
                        },
                        ""timestamp"": ""2018-07-02T15:06:00.0000000Z"",
                        ""isReliable"": false
                    },
                    {
                        ""value"": {
                            ""value"": 0.0,
                            ""units"": ""https://localhost/API/api/v1/enumSets/6/members/0""
                        },
                        ""timestamp"": ""2018-07-02T16:46:00.0000000Z"",
                        ""isReliable"": false
                    },
                    {
                        ""value"": {
                            ""value"": 0.0,
                            ""units"": ""https://localhost/API/api/v1/enumSets/6/members/0""
                        },
                        ""timestamp"": ""2018-07-02T18:26:00.0000000Z"",
                        ""isReliable"": false
                    },
                    {
                        ""value"": {
                            ""value"": 0.0,
                            ""units"": ""https://localhost/API/api/v1/enumSets/6/members/0""
                        },
                        ""timestamp"": ""2018-07-02T20:06:00.0000000Z"",
                        ""isReliable"": false
                    },
                    {
                        ""value"": {
                            ""value"": 0.0,
                            ""units"": ""https://localhost/API/api/v1/enumSets/6/members/0""
                        },
                        ""timestamp"": ""2018-07-02T21:46:00.0000000Z"",
                        ""isReliable"": false
                    },
                    {
                        ""value"": {
                            ""value"": 0.0,
                            ""units"": ""https://localhost/API/api/v1/enumSets/6/members/0""
                        },
                        ""timestamp"": ""2018-07-02T23:26:00.0000000Z"",
                        ""isReliable"": false
                    },
                    {
                        ""value"": {
                            ""value"": 0.0,
                            ""units"": ""https://localhost/API/api/v1/enumSets/6/members/0""
                        },
                        ""timestamp"": ""2018-07-03T01:06:00.0000000Z"",
                        ""isReliable"": false
                    },
                    {
                        ""value"": {
                            ""value"": 0.0,
                            ""units"": ""https://localhost/API/api/v1/enumSets/6/members/0""
                        },
                        ""timestamp"": ""2018-07-03T02:46:00.0000000Z"",
                        ""isReliable"": false
                    },
                    {
                        ""value"": {
                            ""value"": 0.0,
                            ""units"": ""https://localhost/API/api/v1/enumSets/6/members/0""
                        },
                        ""timestamp"": ""2018-07-03T04:26:00.0000000Z"",
                        ""isReliable"": false
                    },
                    {
                        ""value"": {
                            ""value"": 0.0,
                            ""units"": ""https://localhost/API/api/v1/enumSets/6/members/0""
                        },
                        ""timestamp"": ""2018-07-03T06:06:00.0000000Z"",
                        ""isReliable"": false
                    },
                    {
                        ""value"": {
                            ""value"": 0.0,
                            ""units"": ""https://localhost/API/api/v1/enumSets/6/members/0""
                        },
                        ""timestamp"": ""2018-07-03T07:46:00.0000000Z"",
                        ""isReliable"": false
                    },
                    {
                        ""value"": {
                            ""value"": 0.0,
                            ""units"": ""https://localhost/API/api/v1/enumSets/6/members/0""
                        },
                        ""timestamp"": ""2018-07-03T09:26:00.0000000Z"",
                        ""isReliable"": false
                    },
                    {
                        ""value"": {
                            ""value"": 0.0,
                            ""units"": ""https://localhost/API/api/v1/enumSets/6/members/0""
                        },
                        ""timestamp"": ""2018-07-03T11:06:00.0000000Z"",
                        ""isReliable"": false
                    },
                    {
                        ""value"": {
                            ""value"": 0.0,
                            ""units"": ""https://localhost/API/api/v1/enumSets/6/members/0""
                        },
                        ""timestamp"": ""2018-07-03T12:46:00.0000000Z"",
                        ""isReliable"": false
                    },
                    {
                        ""value"": {
                            ""value"": 0.0,
                            ""units"": ""https://localhost/API/api/v1/enumSets/6/members/0""
                        },
                        ""timestamp"": ""2018-07-03T14:26:00.0000000Z"",
                        ""isReliable"": false
                    },
                    {
                        ""value"": {
                            ""value"": 0.0,
                            ""units"": ""https://localhost/API/api/v1/enumSets/6/members/0""
                        },
                        ""timestamp"": ""2018-07-03T16:06:00.0000000Z"",
                        ""isReliable"": false
                    },
                    {
                        ""value"": {
                            ""value"": 0.0,
                            ""units"": ""https://localhost/API/api/v1/enumSets/6/members/0""
                        },
                        ""timestamp"": ""2018-07-03T17:46:00.0000000Z"",
                        ""isReliable"": false
                    }
                ],
                ""attribute"": ""https://localhost/API/api/v1/enumSets/509/members/85"",
                ""self"": ""https://localhost/API.TimeSeriesService/api/v1/points/33333333-3333-3333-3333-333333333333/samples/85?startTime=2018-06-30%2017%3A49%3A03&endTime=2018-07-03%2017%3A49%3A03&page=1&pageSize=1000&sort=timestamp"",
                ""point"": ""https://localhost/API/api/v1/points/33333333-3333-3333-3333-333333333333""
                }";
            }

            return Task.FromResult(JsonConvert.DeserializeObject<BatchCollection<SampleCollectionItem>>(samples));
        }
    }
}
