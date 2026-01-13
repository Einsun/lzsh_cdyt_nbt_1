import { createAPI } from '../requestUtils';

const history = {
  // 设备的增删改查
  request (opts = {}) {
    const { method = 'GET' } = opts
    const paramsKey = method === 'GET' ? 'params' : 'data'
    const {
      [paramsKey]: {
        page = 0,
        pagecount = 10,
        type = 3, //报警类型
        productLine = '', //产线
        commdeviceid = '', //传输设备
        gathertype = '', //设备类型
        name = '', //设备名称
        starttime = '', //开始时间
        endtime = '', //结束时间
      } = {}
    } = opts
    return createAPI(
      {
        url: `AlarmDatas/PageTypeAlarmData`,
        method,
        [paramsKey]: {
          page,
          pagecount,
          type,
          productLine,
          commdeviceid,
          gathertype,
          starttime,
          name,
          endtime,
        }
      }
    )
  },
  // 获取传输设备数据
  getDevice(){
    return createAPI({
      url: 'DashBoard/DeviceSummary',
      method: 'get'
    })
  }
}
export default history
