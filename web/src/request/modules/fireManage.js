import { createAPI } from '../requestUtils';

const fireManage = {
  pushFireMessage (opts = {}) {
    const { data: {
      unitId = '',
      address = ''
    } = {} } = opts
    return createAPI(
      {
        url: '/fire-plan/push-message',
        method: 'POST',
        data: {
          unitId,
          address
        }
      },
    );
  },
  // 设备的增删改查
  miniFireStation (opts = {}) {
    const { method = 'GET' } = opts
    const paramsKey = method === 'GET' ? 'params' : 'data'
    const {
      [paramsKey]: {
        equipment_amount = '',
        equipment_name = '',
        guarantee_date = '',
        id = false,
        guarantee_person = '',
        page = '',
        perPage = '',
        unitID = ''
      } = {}
    } = opts
    return createAPI(
      {
        url: `fire-management/mini-fire-station${id ? '/' + id : ''}`,
        method,
        [paramsKey]: {
          equipment_amount,
          equipment_name,
          guarantee_date,
          guarantee_person,
          page,
          perPage,
          unitID: unitID.replace('#', ''),
        }
      }
    )
  },
  controlRoom (opts = {}) {
    const { method = 'GET' } = opts
    const paramsKey = method === 'GET' ? 'params' : 'data'
    const {
      [paramsKey]: {
        name = '',
        phone = '',
        gender = '',
        job = '',
        id = false,
        page = '',
        perPage = '',
        unitID = ''
      } = {}
    } = opts
    return createAPI(
      {
        url: `fire-management/room-duty${id ? '/' + id : ''}`,
        method,
        [paramsKey]: {
          name,
          phone,
          gender,
          job,
          page,
          perPage,
          unitID: unitID.replace('#', ''),
        }
      }
    )
  },
  importantPlace (opts = {}) {
    const { method = 'GET' } = opts
    const paramsKey = method === 'GET' ? 'params' : 'data'
    const {
      [paramsKey]: {
        name = '',
        position = '',
        overview = '',
        id = false,
        page = '',
        perPage = '',
        unitID = ''
      } = {}
    } = opts
    return createAPI(
      {
        url: `fire-management/key-position${id ? '/' + id : ''}`,
        method,
        [paramsKey]: {
          name,
          position,
          overview,
          page,
          perPage,
          unitID: unitID.replace('#', ''),
        }
      }
    )
  },
  firePlan (opts = {}) {
    const { method = 'GET' } = opts
    const paramsKey = method === 'GET' ? 'params' : 'data'
    const {
      [paramsKey]: {
        organization_name = '',
        organization_duty = '',
        responsible = '',
        phone = '',
        id = false,
        page = '',
        perPage = '',
        upload_id = '',
        unitID = ''
      } = {}
    } = opts
    return createAPI(
      {
        url: `fire-management/fireproof_plan/organization${id ? '/' + id : ''}`,
        method,
        [paramsKey]: {
          organization_name,
          organization_duty,
          responsible,
          phone,
          page,
          perPage,
          upload_id,
          unitID: unitID.replace('#', ''),
        }
      }
    )
  },
  rules (opts = {}) {
    const { method = 'GET' } = opts
    const paramsKey = method === 'GET' ? 'params' : 'data'
    const {
      [paramsKey]: {
        id = '',
        unitID = '',
        rule_name = '',
        make_time = '',
        applicable_range = '',

        page = '',
        perPage = '',
        upload_id = ''
      } = {}
    } = opts
    return createAPI(
      {
        url: `fire-management/rule-system${id ? '/' + id : ''}`,
        method,
        [paramsKey]: {
          id,
          unitID: unitID.replace('#', ''),
          rule_name,
          make_time,
          applicable_range,

          page,
          perPage,
          upload_id
        }
      }
    )
  },
  // 安全指数排名列表
  safetyindex (opts = {}) {
    const { method = 'GET' } = opts
    const paramsKey = method === 'GET' ? 'params' : 'data'
    const {
      [paramsKey]: {
        page = '',
        perPage = '',
      } = {}
    } = opts
    return createAPI(
      {
        url: `unit/dailyScoreList`,
        method,
        [paramsKey]: {
          page,
          perPage,
        }
      }
    )
  },
  // 安全指数排名 历史记录
  dailyScoreHistory (opts = {}) {
    const { method = 'GET' } = opts
    const paramsKey = method === 'GET' ? 'params' : 'data'
    const {
      [paramsKey]: {
        unitID = '',
        page = '',
        perPage = '',
      } = {}
    } = opts
    return createAPI(
      {
        url: `unit/dailyScoreHistory`,
        method,
        [paramsKey]: {
          unitID: unitID.replace('#', ''),
          page,
          perPage,
        }
      }
    )
  },
  // 消防档案
  fireRecord (opts = {}) {
    const { method = 'GET' } = opts // opts.method为空时，默认是get
    const paramsKey = (method === 'GET' ? 'params' : 'data')
    const {
      [paramsKey]: {
        id = false,
        unitID = '',
        file_name = '',
        create_time = '',
        charge_person = '',

        page = '',
        perPage = '',
        upload_id = ''
      } = {}
    } = opts

    return createAPI(
      {
        url: `fire-management/file-manage${id ? '/' + id : ''}`,
        method,
        [paramsKey]: {
          id,
          unitID: unitID.replace('#', ''),
          file_name,
          create_time,
          charge_person,

          page,
          perPage,
          upload_id
        }
      }
    )
  },
  // 设施器材
  fireEquipment (opts = {}) {
    const { method = 'GET' } = opts // opts.method为空时，默认是get
    const paramsKey = (method === 'GET' ? 'params' : 'data')
    const {
      [paramsKey]: {
        id = false,
        unitID = '',
        facility_name = '',
        facility_locale = '',
        keep = '',
        check = '',

        image = '',
        page = '',
        perPage = '',
        upload_id = ''
      } = {}
    } = opts
    return createAPI(
      {
        url: `fire-management/facility${id ? '/' + id : ''}`,
        method,
        [paramsKey]: {
          id,
          unitID: unitID.replace('#', ''),
          facility_name,
          facility_locale,
          keep,
          check,

          image,
          page,
          perPage,
          upload_id
        }
      }
    )
  },
  // 隐患整改
  dangerRectification (opts = {}) {
    const { method = 'GET' } = opts // opts.method为空时，默认是get
    const paramsKey = (method === 'GET' ? 'params' : 'data')
    const {
      [paramsKey]: {
        id = false,
        unitID = '',
        danger_place = '',
        rectification_description = '',
        submitter = '',
        rectification_status = '',
        note = '',
        image = '',
        page = '',
        perPage = '',
      } = {}
    } = opts

    return createAPI(
      {
        url: `fire-management/hidden-danger${id ? '/' + id : ''}`,
        method,
        [paramsKey]: {
          id,
          unitID: unitID.replace('#', ''),
          danger_place,
          rectification_description,
          submitter,
          rectification_status,
          note,
          image,
          page,
          perPage,
        }
      }
    )
  },
  // 领导履责
  // 年度工作计划
  workPlan (opts = {}) {
    const { method = 'GET' } = opts // opts.method为空时，默认是get
    const paramsKey = (method === 'GET' ? 'params' : 'data')
    const {
      [paramsKey]: {
        upload_id = '',
        file_person = '',
        file_name = '',
        page = '',
        unitID = '',
        perPage = '',
      } = {}
    } = opts
    return createAPI(
      {
        url: `fire-management/annual-plan-log`,
        method,
        [paramsKey]: {
          upload_id,
          file_person,
          unitID: unitID.replace('#', ''),
          file_name,
          page,
          perPage,
        }
      }
    )
  },
  // 经费预算
  budget (opts = {}) {
    const { method = 'GET' } = opts // opts.method为空时，默认是get
    const paramsKey = (method === 'GET' ? 'params' : 'data')
    const {
      [paramsKey]: {
        upload_id = '',
        file_person = '',
        file_name = '',
        unitID = '',
        page = '',
        perPage = '',
      } = {}
    } = opts
    return createAPI(
      {
        url: `fire-management/budget-log`,
        method,
        [paramsKey]: {
          upload_id,
          file_person,
          file_name,
          unitID: unitID.replace('#', ''),
          page,
          perPage,
        }
      }
    )
  },
  // 预案
  emergencyPlan (opts = {}) {
    const { method = 'GET' } = opts // opts.method为空时，默认是get
    const paramsKey = (method === 'GET' ? 'params' : 'data')
    const {
      [paramsKey]: {
        upload_id = '',
        file_person = '',
        unitID = '',
        file_name = '',
        page = '',
        perPage = '',
      } = {}
    } = opts
    return createAPI(
      {
        url: `fire-management/emergency-plan-log`,
        method,
        [paramsKey]: {
          upload_id,
          file_person,
          file_name,
          unitID: unitID.replace('#', ''),
          page,
          perPage,
        }
      }
    )
  },
  // 企业图片
  companyImage (opts = {}) {
    const { method = 'GET' } = opts // opts.method为空时，默认是get
    const paramsKey = (method === 'GET' ? 'params' : 'data')
    const {
      [paramsKey]: {
        id = false,
        name = '',
        page = '',
        unitID = '',
        perPage = '',
        upload_id = ''
      } = {}
    } = opts
    return createAPI(
      {
        url: `unit/picture${id ? '/' + id : ''}`,
        method,
        [paramsKey]: {
          id,
          name,
          unitID: unitID.replace('#', ''),
          page,
          perPage,
          upload_id
        }
      }
    )
  },
  // 上传附件
  uploadFile (opts = {}) {
    return createAPI(
      {
        url: `upload`,
        method: 'post',
        ...opts
      }
    )
  },
}
export default fireManage
