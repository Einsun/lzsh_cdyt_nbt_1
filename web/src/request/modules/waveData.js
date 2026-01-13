import { createAPI } from '../requestUtils';
import qs from 'qs'
import request from '@/plugin/axios'

const waveData = {
  // 设备的增删改查
  live(opts = {}) {
    const { method = 'GET' } = opts
    const paramsKey = method === 'GET' ? 'params' : 'data'
    const {
      [paramsKey]: {
        deviceid = '',
      } = {}
    } = opts
    return createAPI(
      {
        url: `AeDatas/LiveWaveData`,
        method,
        [paramsKey]: {
          deviceid,
        }
      }
    )
  },
  getWaveData(opts = {}) {
    console.log('opts--',opts);
    const { method = 'GET' } = opts
    console.log('opts-111-',opts);
    const paramsKey = method === 'GET' ? 'params' : 'data'
    const {
      [paramsKey]: {
        deviceid = 0,
        page = 1,
        pagecount = 100,
      } = {}
    } = opts
    console.log('opts-222-',opts);
    return createAPI(
      {
        url: `/AeDatas/PageWaveData`,
        method,
        [paramsKey]: {
          page,
          pagecount,
          deviceid,
          starttime: opts.starttime,
          endtime: opts.endtime,
        }
      }
    )
  },
  // 获取声发射数据
  getsfsData(id) {
    let method = 'GET'
    const paramsKey = method === 'GET' ? 'params' : 'data'
    return createAPI(
      {
        url: `/AeDatas/AeWaveData`,
        method,
        [paramsKey]: {
          id
        }
      }
    )
  },
  adInfoList(opts = {}) {
    const { method = 'GET' } = opts
    const paramsKey = method === 'GET' ? 'params' : 'data'
    const {
      [paramsKey]: {
        deviceid = 0,
        page = 1,
        pagecount = 10,
      } = {}
    } = opts
    return createAPI(
      {
        url: `AlarmDatas/PageAlarmData`,
        method,
        [paramsKey]: {
          page,
          pagecount,
          deviceid
        }
      }
    )
  },
  settleAlarm(data) {
    return request({
      url: '/AlarmDatas/SettleAlarm',
      method: 'post',
      data: data,
    })
  },
  aeInfoList(opts = {}) {
    const { method = 'GET' } = opts
    const paramsKey = method === 'GET' ? 'params' : 'data'
    const {
      [paramsKey]: {
        deviceid = 0,
        page = 1,
        pagecount = 10,
      } = {}
    } = opts
    return createAPI(
      {
        url: `AlarmDatas/AlarmWavedata`,
        method,
        [paramsKey]: {
          productLineId: deviceid
        }
      }
    )
  },
  aeInfoListMonth(opts = {}) {
    const { method = 'GET' } = opts
    const paramsKey = method === 'GET' ? 'params' : 'data'
    const {
      [paramsKey]: {
        deviceid = 0,
        page = 1,
        pagecount = 10,
      } = {}
    } = opts
    return createAPI(
      {
        url: `AlarmDatas/AlarmWavedataMonth`,
        method,
        [paramsKey]: {
          productLineId: deviceid
        }
      }
    )
  },
  aeInfoListYear(opts = {}) {
    const { method = 'GET' } = opts
    const paramsKey = method === 'GET' ? 'params' : 'data'
    const {
      [paramsKey]: {
        deviceid = 0,
        page = 1,
        pagecount = 10,
      } = {}
    } = opts
    return createAPI(
      {
        url: `AlarmDatas/AlarmWavedataYear`,
        method,
        [paramsKey]: {
          productLineId: deviceid
        }
      }
    )
  },
  aeInfoListDay(opts = {}) {
    const { method = 'GET' } = opts
    const paramsKey = method === 'GET' ? 'params' : 'data'
    const {
      [paramsKey]: {
        deviceid = 0,
        date = ''
      } = {}
    } = opts
    return createAPI(
      {
        url: `AlarmDatas/AlarmWavedataDate`,
        method,
        [paramsKey]: {
          productLineId: deviceid,
          date: date
        }
      }
    )
  },
  aeInfoListTime(opts = {}) {
    const { method = 'GET' } = opts
    const paramsKey = method === 'GET' ? 'params' : 'data'
    const {
      [paramsKey]: {
        deviceid = 0,
        time = '',
        page = 1,
        pagecount = 10,
      } = {}
    } = opts
    return createAPI(
      {
        url: `AlarmDatas/AlarmWavedataTime`,
        method,
        [paramsKey]: {
          productLineId: deviceid,
          time: time
        }
      }
    )
  },
  download(opts = {}) {
    const { method = 'GET' } = opts
    const paramsKey = method === 'GET' ? 'params' : 'data'
    const {
      [paramsKey]: {
        id = '',
      } = {}
    } = opts
    return createAPI(
      {
        url: `AeDatas/Download`,
        method,
        [paramsKey]: {
          id,
        }
      }
    )
  },
  // downloadMuti(data) {
  //   return createAPI({
  //     url: `AeDatas/DownloadMuti`,
  //     method: 'POST',
  //     data: qs.stringify(data, { arrayFormat: 'repeat' })
  //     // data
  //   });
  // },
  downloadMuti(data) {
    return request({
      url: '/AeDatas/DownloadMuti',
      method: 'post',
      data: data,
    })
  },
}
export default waveData
