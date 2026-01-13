import { createAPI } from '../requestUtils';

const alarmRule = {
  // 设备的增删改查
  request (opts = {}) {
    const { method = 'GET' } = opts
    const paramsKey = method === 'GET' ? 'params' : 'data'
    const {
      [paramsKey]: {
        id = false,
        name = '',
        type = '',
        max = '',
        min = '',
      } = {}
    } = opts
    return createAPI(
      {
        url: `AlarmRule${id ? '/' + id : ''}`,
        method,
        [paramsKey]: {
          name,
          type,
          max,
          min
        }
      }
    )
  },
  requestTypeRules (opts = {}) {
    const { method = 'GET' } = opts
    const paramsKey = method === 'GET' ? 'params' : 'data'
    const {
      [paramsKey]: {
        productLineId = 0,
        type = 0,
      } = {}
    } = opts
    return createAPI(
      {
        url: `AlarmRule/TypeRules`,
        method,
        [paramsKey]: {
          productLineId,
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
        type = 0,
        max = '',
        min = '',
        level = '',
      } = {}
    } = opts
    return createAPI(
      {
        url: `AlarmRule`,
        method,
        [paramsKey]: {
          id,
          name,
          type,
          max,
          min,
          level
        }
      }
    )
  },
}
export default alarmRule
