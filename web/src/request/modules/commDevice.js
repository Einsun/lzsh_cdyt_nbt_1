import { createAPI } from '../requestUtils';

const commDevices = {
  // 设备的增删改查
  request (opts = {}) {
    const { method = 'GET' } = opts
    const paramsKey = method === 'GET' ? 'params' : 'data'
    const {
      [paramsKey]: {
        id = false,
        name = '',
        productLineId = 0,
        type = 0,
        ip = '',
        gatherRuleId='',
        sn = ''
      } = {}
    } = opts
    return createAPI(
      {
        url: `CommDevices${id ? '/' + id : ''}`,
        method,
        [paramsKey]: {
          name,
          productLineId,
          gatherRuleId,
          type,
          ip,
          sn,
        }
      }
    )
  },
  requestProductLine (opts = {}) {
    const { method = 'GET' } = opts
    const paramsKey = method === 'GET' ? 'params' : 'data'
    const {
      [paramsKey]: {
        id = false,
        type = 0,
      } = {}
    } = opts
    return createAPI(
      {
        url: `CommDevices/ProductLine${id ? '/' + id : ''}`,
        method,
        [paramsKey]: {
          type,
        }
      }
    )
  },
  requestTypeCommDevice (opts = {}) {
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
        url: `CommDevices/TypeCommDevice`,
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
        productLineId = 0,
        type = 0,
        ip = '',
        sn = '',
        gatherRuleId='',
      } = {}
    } = opts
    return createAPI(
      {
        url: `CommDevices`,
        method,
        [paramsKey]: {
          id,
          name,
          productLineId,
          gatherRuleId,
          type,
          ip,
          sn
        }
      }
    )
  },
}
export default commDevices
