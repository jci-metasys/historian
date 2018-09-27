using HistoricalDataFetcher.Classes.Models.Collection;
using HistoricalDataFetcher.Classes.Models.Collection.TimeSeries;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace HistoricalDataFetcher.Tests
{
    public class NetworkDiscoveryTests
    {
        //[TestInitialize]
        //public void NetworkDiscoveryTestsInit()
        //{
        //    var timeSeriesSetup = new Mock<TimeSeriesEndPoint>(null) { CallBase = true };
        //    //EnumSetService.Instance.EnumFile = @"..\..\..\TimeSeriesTestsEnumList.csv";

        //    //_fqrList = new List<FqrModel>
        //    //{
        //    //    new FqrModel
        //    //    {
        //    //        Guid = new Guid("11111111-1111-1111-1111-111111111111"),
        //    //        Fqr = "test:item-1"
        //    //    },
        //    //    new FqrModel
        //    //    {
        //    //        Guid = new Guid("22222222-2222-2222-2222-222222222222"),
        //    //        Fqr = "test:item-2"
        //    //    },
        //    //    new FqrModel
        //    //    {
        //    //        Guid = new Guid("33333333-3333-3333-3333-333333333333"),
        //    //        Fqr = "test:item-3"
        //    //    }
        //    //};

        //    #region Setup Mock Request Object
        //    ////Setup Network Devices
        //    //timeSeriesSetup.Setup(x =>
        //    //        x.GetCollectionAsync<NetworkDeviceCollectionItem>(It.Is<string>(y => y.ToLower().Contains("/networkdevices?".ToLower()))))
        //    //    .Returns(NetworkDeviceMoqHelper());

        //    ////Setup Point
        //    //timeSeriesSetup.Setup(x =>
        //    //        x.GetCollectionAsync<PointBatchCollectionItem>(It.Is<string>(y => y.ToLower().Contains("/uses".ToLower()))))
        //    //    .Returns(PointMoqHelper());

        //    ////Setup Available Samples
        //    //timeSeriesSetup.Setup(x =>
        //    //        x.GetCollectionAsync<AvailableSampleCollectionItem>(It.Is<string>(y => y.ToLower().EndsWith("/44444444-4444-4444-4444-444444444444/samples".ToLower()))))
        //    //    .Returns(AvailableSamplesMoqHelper());

        //    //Setup Samples
        //    timeSeriesSetup.Setup(x =>
        //            x.GetCollectionAsync<SampleCollectionItem>(It.Is<string>(y => y.ToLower().Contains("/85?".ToLower()))))
        //        .Returns(SamplesMoqHelper());

        //    timeSeriesSetup.Setup(x => x.SaveDataAsync()).Returns(Task.FromResult(true));

        //    #endregion

        //    #region Mock FRQs
        //    //timeSeriesSetup.Setup(x =>
        //    //        x.GetSingleItemAsync<PointBatchCollectionItem>(It.Is<string>(y => y.ToLower().EndsWith("/points/11111111-1111-1111-1111-111111111111".ToLower()))))
        //    //    .Returns(FqrPointMoqHelper("11111111-1111-1111-1111-111111111111"));

        //    //timeSeriesSetup.Setup(x =>
        //    //        x.GetSingleItemAsync<PointBatchCollectionItem>(It.Is<string>(y => y.ToLower().EndsWith("/points/22222222-2222-2222-2222-222222222222".ToLower()))))
        //    //    .Returns(FqrPointMoqHelper("22222222-2222-2222-2222-222222222222"));

        //    //timeSeriesSetup.Setup(x =>
        //    //        x.GetSingleItemAsync<PointBatchCollectionItem>(It.Is<string>(y => y.ToLower().EndsWith("/points/33333333-3333-3333-3333-333333333333".ToLower()))))
        //    //    .Returns(FqrPointMoqHelper("33333333-3333-3333-3333-333333333333"));

        //    //timeSeriesSetup.Setup(x =>
        //    //        x.GetCollectionAsync<AvailableSampleCollectionItem>(It.Is<string>(y => y.ToLower().EndsWith("/11111111-1111-1111-1111-111111111111/samples".ToLower()))))
        //    //    .Returns(FqrAvailableSamplesMoqHelper("11111111-1111-1111-1111-111111111111"));

        //    //timeSeriesSetup.Setup(x =>
        //    //        x.GetCollectionAsync<AvailableSampleCollectionItem>(It.Is<string>(y => y.ToLower().EndsWith("/22222222-2222-2222-2222-222222222222/samples".ToLower()))))
        //    //   .Returns(FqrAvailableSamplesMoqHelper("22222222-2222-2222-2222-222222222222"));

        //    //timeSeriesSetup.Setup(x =>
        //    //        x.GetCollectionAsync<AvailableSampleCollectionItem>(It.Is<string>(y => y.ToLower().EndsWith("/33333333-3333-3333-3333-333333333333/samples".ToLower()))))
        //    //   .Returns(FqrAvailableSamplesMoqHelper("33333333-3333-3333-3333-333333333333"));

        //    timeSeriesSetup.Setup(x =>
        //            x.GetCollectionAsync<SampleCollectionItem>(It.Is<string>(y => y.ToLower().Contains("/objects/11111111-1111-1111-1111-111111111111/attributes/85/samples".ToLower()))))
        //        .Returns(FqrSamplesMoqHelper("11111111-1111-1111-1111-111111111111"));

        //    timeSeriesSetup.Setup(x =>
        //            x.GetCollectionAsync<SampleCollectionItem>(It.Is<string>(y => y.ToLower().Contains("/objects/22222222-2222-2222-2222-222222222222/attributes/85/samples".ToLower()))))
        //        .Returns(FqrSamplesMoqHelper("22222222-2222-2222-2222-222222222222"));

        //    timeSeriesSetup.Setup(x =>
        //            x.GetCollectionAsync<SampleCollectionItem>(It.Is<string>(y => y.ToLower().Contains("/objects/33333333-3333-3333-3333-333333333333/attributes/85/samples".ToLower()))))
        //        .Returns(FqrSamplesMoqHelper("33333333-3333-3333-3333-333333333333"));

        //    timeSeriesSetup.Setup(x => x.SaveDataAsync()).Returns(Task.FromResult(true));
        //    #endregion

        //    //_timeSeriesEndPoint = timeSeriesSetup.Object;

        //    var cacheMock = new Mock<ICache>();
        //    ApiRequest.InitializeAsync(cacheMock.Object, "test", "password", "localhost").Wait();
        //}

        #region Without Fqrs Mock Setup
        private static Task<BatchCollection<NetworkDeviceCollectionItem>> NetworkDeviceMoqHelper()
        {
            return Task.FromResult(JsonConvert.DeserializeObject<BatchCollection<NetworkDeviceCollectionItem>>(
                 @"{
                        ""total"": 1,
                        ""next"": null,
                        ""previous"": null,
                        ""items"": [
                            {
                                ""id"": ""f55a7799-ec10-5361-8569-04258bdd8070"",
                                ""itemReference"": ""pc1:NAE5510"",
                                ""name"": ""NAE5510"",
                                ""type"": ""https://localhost/API/api/v1/enumSets/508/members/185"",
                                ""description"": ""Auth Cat Fire"",
                                ""firmwareVersion"": ""8.0.0.0449"",
                                ""category"": ""https://localhost/API/api/v1/enumSets/33/members/1"",
                                ""timeZone"": ""https://localhost/API/api/v1/enumSets/576/members/53"",
                                ""self"": ""https://localhost/API/api/v1/networkDevices/f55a7799-ec10-5361-8569-04258bdd8070"",
                                ""isChildOf"":  ""https://localhost/API/api/v1/networkDevices/f55a7799-ec10-5361-8569-04258bdd8070/isChildOf"",
                                ""hasChildren"": ""https://localhost/API/api/v1/networkDevices/f55a7799-ec10-5361-8569-04258bdd8070/hasChildren"",
                                ""hosts"": ""https://localhost/API/api/v1/networkDevices/f55a7799-ec10-5361-8569-04258bdd8070/hosts"",
                                ""hostedBy"": ""https://localhost/API/api/v1/networkDevices/f55a7799-ec10-5361-8569-04258bdd8070/hostedBy"",
                                ""uses"": ""https://localhost/API/api/v1/networkDevices/f55a7799-ec10-5361-8569-04258bdd8070/uses"",
                                ""samples"":""https://localhost/API.TimeSeriesService/api/v1/networkdevices/f55a7799-ec10-5361-8569-04258bdd8070/samples"",
                                ""alarms"": """"
                            } 
                        ]
                    }"
             ));


        }

        private static Task<BatchCollection<PointBatchCollectionItem>> PointMoqHelper()
        {
            return Task.FromResult(JsonConvert.DeserializeObject<BatchCollection<PointBatchCollectionItem>>(
                @"{
                    ""total"": 1,
                    ""next"": null,
                    ""previous"": null,
                    ""items"": [
                        {
                            ""id"": ""44444444-4444-4444-4444-444444444444"",
                            ""itemReference"": ""test:item-4"",
                            ""name"": ""Test4"",
                            ""type"": ""https://localhost/API/api/v1/enumSets/508/members/855"",
                            ""self"": ""https://localhost/API/api/v1/points/44444444-4444-4444-4444-444444444444"",
                            ""isChildOf"": ""https://localhost/API/api/v1/points/44444444-4444-4444-4444-444444444444/isChildOf"",
                            ""hasChildren"": ""https://localhost/API/api/v1/points/44444444-4444-4444-4444-444444444444/hasChildren"",
                            ""usedByNetworkDevice"": ""https://localhost/API/api/v1/points/44444444-4444-4444-4444-444444444444/usedByNetworkDevice"",
                            ""hasEquipmentMappings"": ""https://localhost/API/api/v1/points/44444444-4444-4444-4444-444444444444/hasEquipmentMappings"",
                            ""samples"": ""https://localhost/API.TimeSeriesService/api/v1/points/44444444-4444-4444-4444-444444444444/samples""
                        }
                    ]
                }"
            ));
        }

        private static Task<BatchCollection<AvailableSampleCollectionItem>> AvailableSamplesMoqHelper()
        {
            return Task.FromResult(JsonConvert.DeserializeObject<BatchCollection<AvailableSampleCollectionItem>>(
                @"{
                    ""items"": [
                        {
                            ""href"": ""https://localhost/API.TimeSeriesService/api/v1/points/44444444-4444-4444-4444-444444444444/samples/85?startTime=2018-07-01%2012%3A40%3A51&endTime=2018-07-02%2012%3A40%3A51&page=1&pageSize=1000&sort=timestamp"",
                            ""attribute"": ""https://localhost/API/api/v1/enumSets/509/members/85""
                        }
                    ]
                }"
            ));
        }

        private static Task<BatchCollection<SampleCollectionItem>> SamplesMoqHelper()
        {
            return Task.FromResult(JsonConvert.DeserializeObject<BatchCollection<SampleCollectionItem>>(
                @"{
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
                    ""self"": ""https://localhost/API.TimeSeriesService/api/v1/points/88b06228-e09b-59a0-a7c7-ff67d0f318bd/samples/85?startTime=2018-06-30%2017%3A49%3A03&endTime=2018-07-03%2017%3A49%3A03&page=1&pageSize=1000&sort=timestamp"",
                    ""point"": ""https://localhost/API/api/v1/points/88b06228-e09b-59a0-a7c7-ff67d0f318bd""
                }"
            ));
        }
        #endregion

        #region Fqr Mock Setup
        private static Task<PointBatchCollectionItem> FqrPointMoqHelper(string guid)
        {
            string points = string.Empty;

            if (guid.Equals("11111111-1111-1111-1111-111111111111"))
            {
                points = @"{
                    ""id"": ""11111111-1111-1111-1111-111111111111"",
                    ""itemReference"": ""test:item-1"",
                    ""name"": ""Test1"",
                    ""type"": ""https://localhost/API/api/v1/enumSets/508/members/85"",
                    ""self"": ""https://localhost/API/api/v1/points/11111111-1111-1111-1111-111111111111"",
                    ""isChildOf"": ""https://localhost/API/api/v1/points/11111111-1111-1111-1111-111111111111/isChildOf"",
                    ""hasChildren"": ""https://localhost/API/api/v1/points/11111111-1111-1111-1111-111111111111/hasChildren"",
                    ""usedByNetworkDevice"": ""https://localhost/API/api/v1/points/11111111-1111-1111-1111-111111111111/usedByNetworkDevice"",
                    ""hasEquipmentMappings"": ""https://localhost/API/api/v1/points/11111111-1111-1111-1111-111111111111/hasEquipmentMappings"",
                    ""samples"": ""https://localhost/API.TimeSeriesService/api/v1/points/11111111-1111-1111-1111-111111111111/samples""
                }";
            }
            else if (guid.Equals("22222222-2222-2222-2222-222222222222"))
            {
                points = @"{
                    ""id"": ""22222222-2222-2222-2222-222222222222"",
                    ""itemReference"": ""test:item-2"",
                    ""name"": ""Test2"",
                    ""type"": ""https://localhost/API/api/v2/enumSets/508/members/85"",
                    ""self"": ""https://localhost/API/api/v2/points/22222222-2222-2222-2222-222222222222"",
                    ""isChildOf"": ""https://localhost/API/api/v2/points/22222222-2222-2222-2222-222222222222/isChildOf"",
                    ""hasChildren"": ""https://localhost/API/api/v2/points/22222222-2222-2222-2222-222222222222/hasChildren"",
                    ""usedByNetworkDevice"": ""https://localhost/API/api/v2/points/22222222-2222-2222-2222-222222222222/usedByNetworkDevice"",
                    ""hasEquipmentMappings"": ""https://localhost/API/api/v2/points/22222222-2222-2222-2222-222222222222/hasEquipmentMappings"",
                    ""samples"": ""https://localhost/API.TimeSeriesService/api/v2/points/22222222-2222-2222-2222-222222222222/samples""
                }";
            }
            else if (guid.Equals("33333333-3333-3333-3333-333333333333"))
            {
                points = @"{
                    ""id"": ""33333333-3333-3333-3333-333333333333"",
                    ""itemReference"": ""test:item-3"",
                    ""name"": ""Test3"",
                    ""type"": ""https://localhost/API/api/v3/enumSets/508/members/85"",
                    ""self"": ""https://localhost/API/api/v3/points/33333333-3333-3333-3333-333333333333"",
                    ""isChildOf"": ""https://localhost/API/api/v3/points/33333333-3333-3333-3333-333333333333/isChildOf"",
                    ""hasChildren"": ""https://localhost/API/api/v3/points/33333333-3333-3333-3333-333333333333/hasChildren"",
                    ""usedByNetworkDevice"": ""https://localhost/API/api/v3/points/33333333-3333-3333-3333-333333333333/usedByNetworkDevice"",
                    ""hasEquipmentMappings"": ""https://localhost/API/api/v3/points/33333333-3333-3333-3333-333333333333/hasEquipmentMappings"",
                    ""samples"": ""https://localhost/API.TimeSeriesService/api/v3/points/33333333-3333-3333-3333-333333333333/samples""
                }";
            }
            return Task.FromResult(JsonConvert.DeserializeObject<PointBatchCollectionItem>(points));
        }

        private static Task<BatchCollection<AvailableSampleCollectionItem>> FqrAvailableSamplesMoqHelper(string guid)
        {
            string availableSamples = string.Empty;

            if (guid.Equals("11111111-1111-1111-1111-111111111111"))
            {
                availableSamples = @"{
                    ""items"": [
                        {
                            ""href"": ""https://localhost/API.TimeSeriesService/api/v1/points/11111111-1111-1111-1111-111111111111/samples/85?startTime=2018-07-01%2012%3A40%3A51&endTime=2018-07-02%2012%3A40%3A51&page=1&pageSize=1000&sort=timestamp"",
                            ""attribute"": ""https://localhost/API/api/v1/enumSets/509/members/85""
                        }
                    ]
                }";
            }
            else if (guid.Equals("22222222-2222-2222-2222-222222222222"))
            {
                availableSamples = @"{
                    ""items"": [
                        {
                            ""href"": ""https://localhost/API.TimeSeriesService/api/v1/points/22222222-2222-2222-2222-222222222222/samples/85?startTime=2018-07-01%2012%3A40%3A51&endTime=2018-07-02%2012%3A40%3A51&page=1&pageSize=1000&sort=timestamp"",
                            ""attribute"": ""https://localhost/API/api/v1/enumSets/509/members/85""
                        }
                    ]
                }";
            }
            else if (guid.Equals("33333333-3333-3333-3333-333333333333"))
            {
                availableSamples = @"{
                    ""items"": [
                        {
                            ""href"": ""https://localhost/API.TimeSeriesService/api/v1/points/33333333-3333-3333-3333-333333333333/samples/85?startTime=2018-07-01%2012%3A40%3A51&endTime=2018-07-02%2012%3A40%3A51&page=1&pageSize=1000&sort=timestamp"",
                            ""attribute"": ""https://localhost/API/api/v1/enumSets/509/members/85""
                        }
                    ]
                }";
            }

            return Task.FromResult(JsonConvert
                .DeserializeObject<BatchCollection<AvailableSampleCollectionItem>>(availableSamples));
        }

        private static Task<BatchCollection<SampleCollectionItem>> FqrSamplesMoqHelper(string guid)
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

        #endregion
    }
}
