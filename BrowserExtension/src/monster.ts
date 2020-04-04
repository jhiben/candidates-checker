import { checker } from './checker';

const _SEARCH_RESULT_TITLE = '[id=linkResumeTitle]';
const _PROFILE_NAME = '.candidateName';

const _ALL = [_SEARCH_RESULT_TITLE, _PROFILE_NAME].join(',');

checker(_ALL, t => t.split('—')[0]);
