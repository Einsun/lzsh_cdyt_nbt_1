import { createAPI } from '../requestUtils';

const gatherRule = {
  // 设备的增删改查
  request (opts = {}) {
    const { method = 'GET' } = opts
    const paramsKey = method === 'GET' ? 'params' : 'data'
    const {
      [paramsKey]: {
        id = false,
        name = '',
        type = '',
        startTime = '',
        endTime = '',
        interval = ''
      } = {}
    } = opts
    return createAPI(
      {
        url: `GatherRule${id ? '/' + id : ''}`,
        method,
        [paramsKey]: {
          name,
          type,
          startTime,
          endTime,
          interval
        }
      }
    )
  },
  requestTypeRules (opts = {}) {
    const { method = 'GET' } = opts
    const paramsKey = method === 'GET' ? 'params' : 'data'
    const {
      [paramsKey]: {
        // productLineId = 0,
        // type = 0,
      } = {}
    } = opts
    return createAPI(
      {
        url: `GatherRule`,
        method,
        [paramsKey]: {
          // productLineId,
          // type,
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
        startTime = '',
        endTime = '',
        interval = ''
      } = {}
    } = opts
    return createAPI(
      {
        url: `GatherRule`,
        method,
        [paramsKey]: {
          id,
          name,
          type,
          startTime,
          endTime,
          interval
        }
      }
    )
  },
}
export default gatherRule
