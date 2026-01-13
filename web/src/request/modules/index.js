import { createAPI } from '../requestUtils';
// 首页
const index = {
  // summary
  getSummaryData (opts = {}) {
    return createAPI({
      url: 'dashboard/summary',
      method: 'GET'
    })
  },
  // left
  getDeviceCount (opts = {}) {
    return createAPI(
      {
        url: 'DashBoard/DeviceSummary',
        method: 'GET',
      },
    );
  },
  getAlarmTrend (opts = {}) {
    return createAPI(
      {
        url: 'dashboard/chart',
        method: 'GET',
      },
    );
  },
  getHiddenDangerList (opts = {}) {
    return createAPI(
      {
        url: 'unit/hidden-danger-list',
        method: 'GET',
      },
    );
  },
  // footer
  // right
  getIndustryAlarmStatistics (opts = {}) {
    return createAPI(
      {
        url: 'DashBoard/AlarmSummary',
        method: 'GET',
      },
    );
  },
  getAlarmDevice (opts = {}) {
    return createAPI(
      {
        url: 'DashBoard/DeviceSummary',
        method: 'GET',
      },
    );
  },
}

export default index
