import { m } from 'framer-motion';
import { Link as RouterLink } from 'react-router-dom';
// @mui
import { styled } from '@mui/material/styles';
import { Box, Button, Typography, Container } from '@mui/material';
// components
import Page from '../components/Page';
// import { MotionContainer, varBounce } from '../components/animate';
// assets
// import { PageNotFoundIllustration } from '../assets';
import useLocales from '../hooks/useLocales';
// ----------------------------------------------------------------------

const RootStyle = styled('div')(({ theme }) => ({
  
  display: 'flex',
  minHeight: '100%',
  alignItems: 'center',
  paddingTop: theme.spacing(15),
  paddingBottom: theme.spacing(10),
}));

// ----------------------------------------------------------------------

export default function Page404() {
const { translate } = useLocales();

  return (
    <Page title="404 Page Not Found" sx={{ height: 1 }}>
      <RootStyle>
        <Container >
          <Box sx={{ maxWidth: 480, margin: 'auto', textAlign: 'center' }}>
            {/* <m.div variants={varBounce().in}>
              <Typography variant="h3" paragraph>
                {translate('Sorry, page not found!')}
              </Typography>
            </m.div> */}
            <Typography  sx={{ color: 'text.secondary' }}>
              {translate('Sorry, we couldn’t find the page you’re looking for. Perhaps you’ve mistyped the URL? Be sure to check your spelling.')}
            </Typography>

            {/* <m.div variants={varBounce().in}>
              {/* <PageNotFoundIllustration sx={{ height: 260, my: { xs: 5, sm: 10 } }} /> */}
            {/* </m.div> */}

            <Button to="/" size="large" variant="contained" component={RouterLink}>
                {translate('Go to Home')}
              
            </Button>
          </Box>
        </Container>
      </RootStyle>
    </Page>
  );
}
