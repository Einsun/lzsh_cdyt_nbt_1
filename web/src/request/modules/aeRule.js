import { createAPI } from '../requestUtils';

const aeRules = {
  // 设备的增删改查
  request (opts = {}) {
    const { method = 'GET' } = opts
    const paramsKey = method === 'GET' ? 'params' : 'data'
    const {
      [paramsKey]: {
        id = false,
        name = '',
        measureConfig = null,
        levelConfig = null,
        timingConfig = null,
      } = {}
    } = opts
    return createAPI(
      {
        url: `AeRules${id ? '/' + id : ''}`,
        method,
        [paramsKey]: {
          name,
          measureConfig,
          levelConfig,
          timingConfig,
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
        measureConfig = null,
        levelConfig = null,
        timingConfig = null,
      } = {}
    } = opts
    return createAPI(
      {
        url: `AeRules`,
        method,
        [paramsKey]: {
          id,
          name,
          measureConfig,
          levelConfig,
          timingConfig,
        }
      }
    )
  },
}
export default aeRules
