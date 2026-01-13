import { createAPI } from '../requestUtils'
const report = {
  getMonthReport (opts = {}) {
    const {
      params: {
        unitID = '',
        month = '',
        year = '',
        page = '',
        perPage = ''
      } = {} } = opts
    return createAPI(
      {
        url: 'dashboard/getMonthReport',
        method: 'get',
        params: {
          unitID,
          month,
          year,
          page,
          perPage
        }
      },
    );
  },
  getIndustryReportData (opts = {}) {
    const {
      params: {
        unitID = '',
        month = '',
        year = '',
        page = '',
        perPage = ''
      } = {} } = opts
    return createAPI(
      {
        url: 'dashboard/getIndustryReport',
        method: 'get',
        params: {
          unitID,
          month,
          year,
          page,
          perPage
        }
      },
    );
  }
}
export default report
