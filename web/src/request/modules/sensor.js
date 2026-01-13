import { createAPI } from '../requestUtils';

const sensor = {
  // 设备的增删改查
  line (opts = {}) {
    const { method = 'GET' } = opts
    const paramsKey = method === 'GET' ? 'params' : 'data'
    const {
      [paramsKey]: {
        productlineid = 0,
      } = {}
    } = opts
    return createAPI(
      {
        url: `SensorDatas/ProdectLine`,
        method,
        [paramsKey]: {
          productlineid
        }
      }
    )
  },
}
export default sensor
