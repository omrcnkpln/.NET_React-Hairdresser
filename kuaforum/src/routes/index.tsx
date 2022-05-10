import { Suspense, lazy, ElementType } from 'react';
import { Navigate, useRoutes, useLocation } from 'react-router-dom';
import LoadingScreen from '../components/LoadingScreen';
import AuthGuard from '../guards/AuthGuard';
import GuestGuard from '../guards/GuestGuard';
import LogoOnlyLayout from '../layouts/LogoOnlyLayout';
import Home from '../pages/Home/Home';

// ----------------------------------------------------------------------

const Loadable = (Component: ElementType) => (props: any) => {
  // eslint-disable-next-line react-hooks/rules-of-hooks
  const { pathname } = useLocation();

  return (
    <Suspense fallback={<LoadingScreen isDashboard={pathname.includes('/dashboard')} />}>
      <Component {...props} />
    </Suspense>
  );
};

// element: <AuthGuard><Home /></AuthGuard>,

export default function Router() {
  return useRoutes([
    // {
    //   path: 'auth',
    //   children: [
    //     {
    //       path: 'login',
    //       element: (
    //         <GuestGuard>
    //           <Login />
    //         </GuestGuard>
    //       ),
    //     },
    //     { path: 'login-unprotected', element: <Login /> },
    //     { path: 'reset-password', element: <ResetPassword /> },
    //     { path: 'verify', element: <VerifyCode /> },
    //   ],
    // },
    {
      path: '/',
      element: <Home />,
      //element:  <DashboardLayout /> ,
      children: [
        { element: <Navigate to="/home" replace />, index: true },
        { path: '/home', element: <Home /> },
        
        // {
        //   path: '/raw',
        //   children: [
        //     { element: <Navigate to="/raw/materials" replace />, index: true },
        //     { path: '/raw/materials', element: <RawMaterials /> },
        //     { path: '/raw/materials/edit/:name', element: <CreateRawMetarial /> },
        //     { path: '/raw/materials/create', element: <CreateRawMetarial /> },
        //     { path: '/raw/categories', element: <RawCategories /> },
        //     { path: '/raw/categories/create', element: <CreateCategory /> },
        //     { path: '/raw/categories/edit/:name', element: <CreateCategory /> },

        //   ],
        // },
      ],
    },
    {
      path: '*',
      element: <LogoOnlyLayout />,
      children: [
        { path: '404', element: <NotFound /> },
        { path: '*', element: <Navigate to="/404" replace /> },
      ],
    },
    { path: '*', element: <Navigate to="/404" replace /> },
  ]);
}

// Dashboard
// const RawCategory = Loadable(lazy(() => import('src/pages/dashboard/RawCategory')));
// const RawMaterials = Loadable(lazy(() => import('src/pages/dashboard/RawMetarials')));
// const CreateRawMetarial = Loadable(lazy(() => import('src/pages/dashboard/CreateRawMetarial')));
// const CreateCategory = Loadable(lazy(() => import('src/pages/dashboard/CreateCategory')));
// const GeneralExpenses = Loadable(lazy(() => import('src/pages/dashboard/GeneralExpenses')));
// const MattressTypes = Loadable(lazy(() => import('src/pages/dashboard/MattressTypes')));
// const GeneralSettings = Loadable(lazy(() => import('src/pages/dashboard/GeneralSettings')));
// const CreateMattress = Loadable(lazy(() => import('../pages/dashboard/CreateMattress')));
// const Dashboard = Loadable(lazy(() => import('../pages/dashboard/Dashboard')));
// Authentication
const Login = Loadable(lazy(() => import('../pages/auth/Login')));
const ResetPassword = Loadable(lazy(() => import('../pages/auth/ResetPassword')));
const VerifyCode = Loadable(lazy(() => import('../pages/auth/VerifyCode')));

const NotFound = Loadable(lazy(() => import('../pages/Page404')));
