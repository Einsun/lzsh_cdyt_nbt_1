import { createAPI } from '../requestUtils'
import deviceMockData from '@/mock/requestApi/device'
const mockRequest = (key) => {
  return new Promise((resolve, reject) => {
    resolve(JSON.parse(JSON.stringify(deviceMockData[key])))
  })
}

const device = {
  getFireAlarmList (opts = {}) {
    const { params: {
      startTime = '',
      endTime = '',
      message = '',
      deviceId = '',
      deviceType = '',
      unitID = '',
      alarm_SourceDesc = '',
      page = '',
      perPage = ''
    } = {} } = opts
    return createAPI(
      {
        url: 'device/fire-alarm/list',
        method: 'GET',
        params: {
          startTime,
          endTime,
          page,
          perPage,
          message,
          deviceId,
          deviceType,
          unitID: unitID.replace('#', ''),
          alarm_SourceDesc
        }
      },
    );
  },
  getFireAlarmListByUnit (opts = {}) {
    const { params: {
      startTime = '',
      endTime = '',
      message = '',
      deviceId = '',
      deviceType = '',
      unitID = '',
      alarm_SourceDesc = '',
      page = '',
      perPage = ''
    } = {} } = opts
    return createAPI(
      {
        url: 'device/fire-alarm/unit',
        method: 'GET',
        params: {
          startTime,
          endTime,
          page,
          perPage,
          message,
          deviceId,
          deviceType,
          unitID: unitID.replace('#', ''),
          alarm_SourceDesc
        }
      },
    );
  },
  getFireAlarmHistory (opts = {}) {
    const { params: {
      deviceId = '',
      alarmSourceDesc = '',
      orderByTime = '',
      page = 1,
      unitID = '',
      perPage = 10
    } = {} } = opts
    return createAPI(
      {
        url: 'device/fire-alarm/history',
        method: 'GET',
        params: {
          deviceId,
          alarmSourceDesc,
          orderByTime,
          page,
          unitID: unitID.replace('#', ''),
          perPage
        }
      },
    );
  },
  // 获取本单位楼列表
  getBuildingList (opts = {}) {
    const { params = {} } = opts
    return createAPI(
      {
        url: 'unit/building-list',
        method: 'GET',
        params: {
        }
      },
    );
  },
  // Web - 点位表 Excel 导入
  uploadDevice (opts = {}) {
    const {
      data = {}
    } = opts
    return createAPI({
      url: `device-point-position/import-excel`,
      method: 'POST',
      header: { 'content-type': 'multipart/form-data' },
      data
    }
    )
  },
  // 电气
  getElectronicFireList (opts = {}) {
    const { params: {
      deviceAddr = '',
      deviceName = '',
      deviceStatus = '',
      unitID = '',
      deviceNo = '',
      page = '',
      perPage = ''
    } = {} } = opts
    return createAPI(
      {
        url: 'device/electronic-fire/list',
        method: 'GET',
        params: {
          deviceAddr,
          deviceName,
          deviceStatus,
          unitID: unitID.replace('#', ''),
          deviceNo,
          page,
          perPage
        }
      }
    )
  },
  getElectronicFireSummary (opts = {}) {
    return createAPI(
      {
        url: 'device/electronic-fire/summary',
        method: 'GET',
      }
    )
  },
  getElectronicFireDetails (opts = {}) {
    const { params: {
      IMSI = '',
      page = '',
      perPage = '',
      unitID = '',
      type = '',
      orderByTime = ''
    } = {} } = opts
    return createAPI(
      {
        url: 'device/electronic-fire/history',
        method: 'GET',
        params: {
          IMSI,
          orderByTime,
          type,
          unitID: unitID.replace('#', ''),
          page,
          perPage
        }
      }
    )
  },
  //  水压表
  getWaterGapeList (opts = {}) {
    const { params: {
      deviceNo = '',
      deviceAddress = '',
      status = '',
      IMSI = '',
      page = '',
      perPage = '',
      unitID = '',
      orderByTime = ''
    } = {} } = opts
    return createAPI(
      {
        url: 'device/water-gage/list',
        method: 'GET',
        params: {
          type: 1,
          deviceType: 1,
          deviceNo,
          deviceAddress,
          status,
          imei: deviceNo,
          orderByTime,
          unitID: unitID.replace('#', ''),
          page,
          perPage
        }
      }
    )
  },
  getWaterSprayList (opts = {}) {
    const { params: {
      deviceNo = '',
      deviceAddress = '',
      status = '',
      IMSI = '',
      page = '',
      perPage = '',
      unitID = '',
      orderByTime = ''
    } = {} } = opts
    return createAPI(
      {
        url: 'device/water-gage/list',
        method: 'GET',
        params: {
          type: 1,
          deviceType: 2,
          IMSI,
          deviceNo,
          deviceAddress,
          status,
          imei: deviceNo,
          orderByTime,
          unitID: unitID.replace('#', ''),
          page,
          perPage
        }
      }
    )
  },
  getWaterSprayDetails (opts = {}) {
    const { params: {
      deviceNo = '',
      page = '',
      perPage = '',
      orderByTime = '',
      unitID = '',
    } = {} } = opts
    return createAPI(
      {
        url: 'device/water-gage/history',
        method: 'GET',
        params: {
          type: 2,
          deviceNo,
          page,
          perPage,
          unitID: unitID.replace('#', ''),
          orderByTime
        }
      }
    )
  },
  getWaterGapeDetails (opts = {}) {
    const { params: {
      deviceNo = '',
      page = '',
      perPage = '',
      orderByTime = '',
      unitID = '',
    } = {} } = opts
    return createAPI(
      {
        url: 'device/water-gage/history',
        method: 'GET',
        params: {
          type: 1,
          deviceNo,
          page,
          perPage,
          unitID: unitID.replace('#', ''),
          orderByTime
        }
      }
    )
  },
  getWaterGapeTrend (opts = {}) {
    const { params: {
      deviceNo = '',
      page = '',
      perPage = '',
      unitID = '',
    } = {} } = opts
    const mock = true
    if (mock) {
      return mockRequest('getWaterGapeTrend')
    } else {
      return createAPI(
        {
          url: 'device/water-gage/history',
          method: 'GET',
          params: {
            type: 1,
            deviceNo,
            page,
            unitID: unitID.replace('#', ''),
            perPage
          }
        }
      )
    }
  },
  //  水位表
  getWaterLevelList (opts = {}) {
    const { params: {
      deviceNo = '',
      deviceAddress = '',
      status = '',
      IMSI = '',
      page = '',
      perPage = '',
      unitID = '',
      orderByTime = ''
    } = {} } = opts
    return createAPI(
      {
        url: 'device/water-gage/list',
        method: 'GET',
        params: {
          type: 2,
          IMSI,
          deviceNo,
          deviceAddress,
          status,
          imei: deviceNo,
          orderByTime,
          unitID: unitID.replace('#', ''),
          page,
          perPage
        }
      }
    )
  },
  getWaterLevelDetails (opts = {}) {
    const { params: {
      deviceNo = '',
      page = '',
      perPage = '',
      orderByTime = ''
    } = {} } = opts
    return createAPI(
      {
        url: 'device/water-gage/history',
        method: 'GET',
        params: {
          type: 2,
          deviceNo,
          page,
          perPage,
          orderByTime
        }
      }
    )
  },
  getWaterLevelTrend (opts = {}) {
    const { params: {
      deviceNo = '',
      page = '',
      perPage = ''
    } = {} } = opts
    const mock = true
    if (mock) {
      return mockRequest('getWaterLevelTrend')
    } else {
      return createAPI(
        {
          url: 'device/water-gage/history',
          method: 'GET',
          params: {
            type: 2,
            deviceNo,
            page,
            perPage
          }
        }
      )
    }
  },
  //  水泵表
  getWaterPumpList (opts = {}) {
    // const {
    //   params: {
    //
    //   } = {}
    // } = opts
    return createAPI(
      {
        url: 'device/water-pump/management-list',
        method: 'GET',
        params: {
        }
      }
    )
  },
  // 烟感
  getSmokeDetectorList (opts = {}) {
    const {
      params: {
        sn = '',
        location = '',
        status = '',
        unitID = '',
        page = '',
        perPage = '',
      }
    } = opts
    return createAPI(
      {
        url: 'device/smoke-detector/list',
        method: 'GET',
        params: {
          sn,
          location,
          status,
          unitID: unitID.replace('#', ''),
          page,
          perPage
        }
      }
    )
  },
  getSmokeDetectorDetails (opts = {}) {
    const {
      params: {
        deviceNo = '',
        page = '',
        perPage = '',
        orderByTime = ''
      }
    } = opts
    return createAPI(
      {
        url: 'device/smoke-detector/history',
        method: 'GET',
        params: {
          deviceNo,
          orderByTime,
          page,
          perPage
        }
      }
    )
  },
  getVideoDeviceList (opts = {}) {
    const { params: {
      unitName = '',
      cameraSN = '',
      unitID = '',
    } = {} } = opts
    return createAPI(
      {
        url: 'device/video/list',
        method: 'GET',
        params: {
          unitName,
          cameraSN,
          unitID: unitID.replace('#', ''),
        }
      }
    )
  },
  getDeviceOverview (opts = {}) {
    const { params: {
      unitID = '',
      page = '',
      perPage = '',
    } = {} } = opts
    return createAPI(
      {
        url: 'device/count/overview',
        method: 'GET',
        params: {
          page,
          perPage,
          unitID: unitID.replace('#', ''),
        }
      }
    )
  },
  getAlarmOverview (opts = {}) {
    const { params: {
      unitID = '',
      page = '',
      perPage = '',
    } = {} } = opts
    return createAPI(
      {
        url: 'dashboard/getUnitAlarmTotalList',
        method: 'GET',
        params: {
          page,
          perPage,
          unitID: unitID.replace('#', ''),
        }
      }
    )
  },
}
export default device
