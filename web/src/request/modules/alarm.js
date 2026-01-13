import { createAPI } from '../requestUtils'
const alarm = {
  // alarm
  getAlarmList (opts = {}) {
    return createAPI(
      Object.assign(
        {
          url: 'alarm-records',
          method: 'GET'
        },
        opts
      )
    );
  },
  // sn-alarm-push-record
  getAlarmListFromSn (opts = {}) {
    return createAPI(
      Object.assign(
        {
          url: 'alarm-records-sn',
          method: 'GET'
        },
        opts
      )
    );
  },
  postAlarmMessage (opts = {}) {
    return createAPI(
      Object.assign(
        {
          url: 'alarm-sms',
          method: 'POST'
        },
        opts
      )
    );
  },
  handleAlarm (opts = {}) {
    const { data: {
      id = '',
      cause = '',
      category = ''
    } = {} } = opts
    return createAPI(
      {
        url: 'alarm-handle',
        method: 'POST',
        data: {
          id,
          cause,
          category
        }
      },
    );
  },
  rebootAlarm (opts = {}) {
    const { data: {
      id = ''
    } = {} } = opts
    return createAPI(
      {
        url: 'unit/reboot-alarm',
        method: 'POST',
        data: {
          id
        }
      },
    );
  },
  handleSmokeAlarm (opts = {}) {
    const { data: {
      id = '',
      cause = '',
      category = ''
    } = {} } = opts
    return createAPI(
      {
        url: 'alarm-smoke-handle',
        method: 'POST',
        data: {
          id,
          cause,
          category
        }
      },
    );
  },
  startFirePlan (opts = {}) {
    const { data: {
      id = '',
      cause = '',
      category = ''
    } = {} } = opts
    return createAPI(
      {
        url: 'start-firePlan',
        method: 'POST',
        data: {
          id,
          cause,
          category
        }
      },
    );
  },
}
export default alarm
