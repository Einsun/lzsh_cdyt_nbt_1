import { createAPI } from '../requestUtils';

const patrol = {
  // 建筑的增删改查
  building (opts = {}) {
    const { method = 'GET' } = opts
    const paramsKey = method === 'GET' ? 'params' : 'data'
    const {
      [paramsKey]: {
        building_name = '',
        building_address = '',
        created_at = '',
        id = false,
        page = '',
        perPage = '',
      } = {}
    } = opts
    return createAPI(
      {
        url: `patrol_unit_building${id ? '/' + id : ''}`,
        method,
        [paramsKey]: {
          building_name,
          building_address,
          created_at,
          page,
          perPage,
        }
      }
    )
  },
  // 计划的增删改查
  plan (opts = {}) {
    const { method = 'GET' } = opts // opts.method为空时，默认是get
    const paramsKey = (method === 'GET' ? 'params' : 'data')
    const {
      [paramsKey]: {
        title = '',
        comment = '',
        cycle_unit = '1', // 默认1是天
        cycle_num = '',
        created_at = '',
        id = false,
        page = '',
        perPage = '',
      } = {}
    } = opts

    return createAPI(
      {
        url: `patrol_plan${id ? '/' + id : ''}`,
        method,
        [paramsKey]: {
          title,
          comment,
          cycle_unit,
          cycle_num,
          created_at,
          page,
          perPage,
        }
      }
    )
  },
  // 巡检点的增删改查
  spot (opts = {}) {
    const { method = 'GET' } = opts // opts.method为空时，默认是get
    const paramsKey = (method === 'GET' ? 'params' : 'data')
    const {
      [paramsKey]: {
        title = '',
        comment = '',
        floor = '',
        address = '',
        spot_no = '',
        building_id = '',
        plan_id = '',
        created_at = '',
        owner = '',
        _equipment = '', // Array
        id = false,
        page = '',
        perPage = '',
      } = {}
    } = opts
    return createAPI(
      {
        url: `patrol_spot${id ? '/' + id : ''}`,
        method,
        [paramsKey]: {
          title,
          comment,
          floor,
          address,
          spot_no,
          building_id,
          plan_id,
          created_at,
          owner,
          _equipment,
          page,
          perPage,
        }
      }
    )
  },
  // 设备的增删改查
  device (opts = {}) {
    const { method = 'GET' } = opts
    const paramsKey = method === 'GET' ? 'params' : 'data'
    const {
      [paramsKey]: {
        title = '',
        fields = '',
        comment = '',
        id = false,
        created_at = '',
        page = '',
        perPage = '',
      } = {}
    } = opts
    return createAPI(
      {
        url: `patrol_equipment${id ? '/' + id : ''}`,
        method,
        [paramsKey]: {
          title,
          fields,
          comment,
          created_at,
          page,
          perPage,
        }
      }
    )
  },
  getDeviceDetail (opts = {}) {
    const {
      params: {
        id = false
      } = {}
    } = opts
    if (id !== false) {
      return createAPI(
        {
          url: `patrol_equipment${id ? '/' + id : ''}`,
          method: 'GET',
        }
      )
    }
  },
  getPatrolRecordByDayList (opts = {}) {
    const {
      params: {
        page = '',
        perPage = '',
        plan_id = '',
        orderByTime = ''
      } = {}
    } = opts
    return createAPI(
      {
        url: `patrol_plan_record/${plan_id}/days`,
        method: 'GET',
        params: {
          plan_id,
          page,
          perPage,
          orderByTime
        }
      }
    )
  },
  getPatrolDetail (opts = {}) {
    const {
      params: {
        spot_id = '',
        plan_id = '',
        date = ''
      } = {}
    } = opts
    return createAPI(
      {
        url: `patrol_plan_record`,
        method: 'GET',
        params: {
          spot_id,
          plan_id,
          date
        }
      }
    )
  },
  uploadSpot (opts = {}) {
    const {
      data = {}
    } = opts
    return createAPI(
      {
        url: `patrol_spot/import-excel`,
        method: 'POST',
        header: { 'content-type': 'multipart/form-data' },
        data
      }
    )
  },

}
export default patrol
