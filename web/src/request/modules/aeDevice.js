import { createAPI } from '../requestUtils';

const aeDevices = {
  // 设备的增删改查
  request (opts = {}) {
    const { method = 'GET' } = opts
    const paramsKey = method === 'GET' ? 'params' : 'data'
    const {
      [paramsKey]: {
        id = false,
        name = '',
        sn = '',
        productLineId = 0,
        type = 0,
        commDeviceId = 0,
        address = '',
        channel = 0,
        alarmRuleId = 0,
        gatherRuleId = 0,
        state = 0,
        maxValue = 0,
        minValue = 0,
        samping = 0,
        signalTypeId = 0,
        unit = '',
      } = {}
    } = opts
    return createAPI(
      {
        url: `Devices${id ? '/' + id : ''}`,
        method,
        [paramsKey]: {
          name,
          sn,
          productLineId,
          type,
          commDeviceId,
          address,
          channel,
          alarmRuleId,
          gatherRuleId,
          state,
          maxValue,
          minValue,
          samping,
          signalTypeId,
          unit
        }
      }
    )
  },
  aeRequest (opts = {}) {
    const { method = 'GET' } = opts
    const paramsKey = method === 'GET' ? 'params' : 'data'
    const {
      [paramsKey]: {
        id = false,
        name = '',
        sn = '',
        type = 0,
        productLineId = 0,
        commDeviceId = 0,
        aeRuleId = 0,
      } = {}
    } = opts
    return createAPI(
      {
        url: `Devices/AEDevice${id ? '/' + id : ''}`,
        method,
        [paramsKey]: {
          name,
          sn,
          productLineId,
          type,
          commDeviceId,
          aeRuleId,
        }
      }
    )
  },
  aeUpdate (opts = {}) {
    const { method = 'GET' } = opts
    const paramsKey = method === 'GET' ? 'params' : 'data'
    const {
      [paramsKey]: {
        id = '',
        name = '',
        sn = '',
        type = 0,
        productLineId = 0,
        commDeviceId = 0,
        aeRuleId = 0,
      } = {}
    } = opts
    return createAPI(
      {
        url: `Devices/AEDevice`,
        method,
        [paramsKey]: {
          id,
          name,
          sn,
          productLineId,
          type,
          commDeviceId,
          aeRuleId,
        }
      }
    )
  },
  requestDeviceSummary (opts = {}) {
    const { method = 'GET' } = opts
    const paramsKey = method === 'GET' ? 'params' : 'data'
    const {
      [paramsKey]: {
        prodecelineid = 0,
      } = {}
    } = opts
    return createAPI(
      {
        url: `Devices/DeviceSummary`,
        method,
        [paramsKey]: {
          prodecelineid,
        }
      }
    )
  },
  requestProductLine (opts = {}) {
    const { method = 'GET' } = opts
    const paramsKey = method === 'GET' ? 'params' : 'data'
    const {
      [paramsKey]: {
        id = 0,
        type = 0,
      } = {}
    } = opts
    return createAPI(
      {
        url: `Devices/ProductLine`,
        method,
        [paramsKey]: {
          id,
          type,
        }
      }
    )
  },
  update (opts = {}) {
    const { method = 'GET' } = opts
    const paramsKey = method === 'GET' ? 'params' : 'data'
    const {
      [paramsKey]: {
        id = '',
        name = '',
        sn = '',
        productLineId = 0,
        type = 0,
        commDeviceId = 0,
        address = '',
        channel = 0,
        pos = 0,
        maxValue = 0,
        minValue = 0,
        alarmRuleId = 0,
        gatherRuleId = 0,
        aeInfo = '',
        state = 0,
        signalTypeId = 0,
        samping = 0,
        unit = '',
      } = {}
    } = opts
    return createAPI(
      {
        url: `Devices`,
        method,
        [paramsKey]: {
          id,
          name,
          sn,
          productLineId,
          type,
          commDeviceId,
          address,
          channel,
          pos,
          alarmRuleId,
          gatherRuleId,
          aeInfo,
          maxValue,
          minValue,
          state,
          signalTypeId,
          samping,
          unit
        }
      }
    )
  },
}
export default aeDevices
