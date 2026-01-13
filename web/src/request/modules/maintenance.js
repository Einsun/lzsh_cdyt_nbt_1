import { createAPI } from '../requestUtils';

const maintenance = {

  maintenanceTask (opts = {}) {
    const { method = 'GET' } = opts // opts.method为空时，默认是get
    const paramsKey = (method === 'GET' ? 'params' : 'data')
    const {
      [paramsKey]: {
        id = false,
        unitID = '',
        task_name = '',
        task_system = '',
        task_start_time = '',
        task_end_time = '',

        page = '',
        perPage = '',
      } = {}
    } = opts

    return createAPI(
      {
        url: `maintenance${id ? '/' + id : ''}`,
        method,
        [paramsKey]: {
          id,
          task_name,
          task_system,
          task_start_time,
          task_end_time,
          page,
          perPage,
          unitID: unitID.replace('#', ''),
        }
      }
    )
  },
  maintenanceHistoryTask (opts = {}) {
    const { method = 'GET' } = opts // opts.method为空时，默认是get
    const paramsKey = (method === 'GET' ? 'params' : 'data')
    const {
      [paramsKey]: {
        id = false,
        unitID = '',
        task_name = '',
        task_system = '',
        task_start_time = '',
        task_end_time = '',

        page = '',
        perPage = '',
      } = {}
    } = opts

    return createAPI(
      {
        url: `maintenance/getMaintenanceHistoryTasks${id ? '/' + id : ''}`,
        method,
        [paramsKey]: {
          id,
          unitID,
          task_name,
          task_system,
          task_start_time,
          task_end_time,
          page,
          perPage,
        }
      }
    )
  },
}
export default maintenance
